using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class PhysicsSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private readonly int gridId;

        private readonly List<DynamicPack> inactiveDynamics = new List<DynamicPack>();
        private readonly List<DynamicPack> activeDynamics = new List<DynamicPack>();
        private readonly List<int> inactiveStatics = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly IBroadphaseMan broadphaseMan;
        private readonly IFixtureMan fixtureMan;
        private readonly ICollisionMan collisionMan;
        private readonly DynamicHelper dynamicHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal PhysicsSystem(IEntityMan entityMan, IFixtureMan fixtureMan, ICollisionMan collisionMan, IBroadphaseMan broadphaseMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
            this.collisionMan = collisionMan;
            this.broadphaseMan = broadphaseMan;
            this.dynamicHelper = new DynamicHelper(this, entityMan);

            Require<BodyComponent>();
            RegisterHandler<BodyOnCommand>(HandleBodyOnCommand);
            RegisterHandler<BodyOffCommand>(HandleBodyOffCommand);

            gridId = broadphaseMan.CreateGrid(128, 128, 16);
        }

        #endregion Internal Constructors

        #region Public Methods

        public static Vector2 GetCellCenter(PositionComponent pos)
        {
            return new Vector2(pos.Value.X + CELL_SIZE / 2, pos.Value.Y + CELL_SIZE / 2);
        }

        public static Box2 GetCellBox(PositionComponent pos)
        {
            var bx = pos.Value.X;
            var by = pos.Value.Y;

            return new Box2(bx, by, bx + CELL_SIZE, by + CELL_SIZE);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();
        }

        public void Update(float dt)
        {
            ExecuteCommands();

            UpdateAabbs();

            SweepAndPrune(dt);
        }

        #endregion Public Methods

        #region Internal Methods

        internal IFixture GetFixture(int fixtureId)
        {
            return fixtureMan.GetById(fixtureId);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            if (entity.Components.Any(item => item is VelocityComponent))
                RegisterDynamicEntity(entity);
            else
                RegisterStaticEntity(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            if (entity.Components.Any(item => item is VelocityComponent))
                UnregisterDynamicEntity(entity);
            else
                UnregisterStaticEntity(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private static Box2 GetAabb(Entity entity)
        {
            var body = entity.Get<BodyComponent>();
            return body.Aabb;
        }

        private bool HandleBodyOnCommand(BodyOnCommand cmd)
        {
            var dynamicToActivate = inactiveDynamics.FirstOrDefault(item => item.EntityId == cmd.EntityId);

            var entity = entityMan.GetById(cmd.EntityId);

            if (dynamicToActivate != null)
            {
                activeDynamics.Add(dynamicToActivate);
                inactiveDynamics.Remove(dynamicToActivate);
                entity.RaiseEvent(new BodyOnEventArgs(entity));
                return true;
            }

            if (inactiveStatics.Contains(cmd.EntityId))
            {
                InsertToGrid(entity);
                inactiveStatics.Remove(cmd.EntityId);
                entity.RaiseEvent(new BodyOnEventArgs(entity));
                return true;
            }

            return false;
        }

        private bool HandleBodyOffCommand(BodyOffCommand cmd)
        {
            var dynamicToDeactivate = activeDynamics.FirstOrDefault(item => item.EntityId == cmd.EntityId);

            var entity = entityMan.GetById(cmd.EntityId);

            if (dynamicToDeactivate != null)
            {
                inactiveDynamics.Add(dynamicToDeactivate);
                activeDynamics.Remove(dynamicToDeactivate);

                entity.RaiseEvent(new BodyOffEventArgs(entity));
                return true;
            }

            RemoveFromGrid(entity);
            inactiveStatics.Add(entity.Id);
            entity.RaiseEvent(new BodyOffEventArgs(entity));
            return true;
        }

        private void UpdateAabb(BodyComponent body, PositionComponent pos)
        {
            var fixture = fixtureMan.GetById(body.Fixtures.First());
            body.Aabb = fixture.Shape.GetAabb().Translated(pos.Value);
        }

        private void UpdateAabbs()
        {
            for (int i = 0; i < activeDynamics.Count; i++)
            {
                var ad = activeDynamics[i];
                UpdateAabb(ad.Body, ad.Position);
            }
        }

        private void SweepAndPrune(float dt)
        {
            var xActiveList = new List<DynamicPack>();
            DynamicPack nextCollider = null;

            activeDynamics.Sort(Xcomparison);

            //Clear dynamics
            for (int i = 0; i < activeDynamics.Count; i++)
            {
                var entity = entityMan.GetById(activeDynamics[i].EntityId);
                entity.DebugData = null;
            }

            for (int i = 0; i < activeDynamics.Count - 1; i++)
            {
                QueryStaticGrid(activeDynamics[i], dt);

                nextCollider = activeDynamics[i + 1];
                xActiveList.Add(activeDynamics[i]);

                for (int j = 0; j < xActiveList.Count; j++)
                {
                    var currentCollider = xActiveList[j];

                    if (nextCollider.Aabb.Left < currentCollider.Aabb.Right)
                    {
                        if (nextCollider.Aabb.Bottom <= currentCollider.Aabb.Top && nextCollider.Aabb.Top > currentCollider.Aabb.Bottom)
                            TestNarrowPhaseDynamic(nextCollider, currentCollider, dt);
                    }
                    else
                    {
                        xActiveList.RemoveAt(j);
                        j--;
                    }
                }
            }

            if (activeDynamics.Count > 0)
                QueryStaticGrid(activeDynamics.Last(), dt);
        }

        private void TestNarrowPhaseDynamic(DynamicPack packA, DynamicPack packB, float dt)
        {
            Vector2 projection;
            if (dynamicHelper.TestVsDynamic(packA, packB, dt, out projection))
            {
                var entityA = entityMan.GetById(packA.EntityId);
                var entityB = entityMan.GetById(packB.EntityId);

                collisionMan.Callback(entityA, entityB, projection);

                //bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                //bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                //DynamicHelper.ResolveVsDynamic(bodyA, bodyB, projection, dt);
            }
        }

        private void TestNarrowPhaseStatic(DynamicPack packA, Entity entityB, float dt)
        {
            var entityA = entityMan.GetById(packA.EntityId);

            Vector2 projection;
            if (dynamicHelper.TestVsStatic(packA, entityB, dt, out projection))
            {
                collisionMan.Callback(entityA, entityB, projection);
                collisionMan.Callback(entityB, entityA, -projection);
            }
        }

        private void QueryStaticGrid(DynamicPack pack, float dt)
        {
            var dynamicAabb = pack.Aabb;

            var idSet = broadphaseMan.QueryStatic(gridId, dynamicAabb);

            if (idSet.Count == 0)
                return;

            var entitySet = idSet.Select(id => entityMan.GetById(id)).ToList();

            var pos = dynamicAabb.GetCenter();

            entitySet.Sort((a, b) => ShortestDistanceComparer(pos, GetCellCenter(a.Get<PositionComponent>()), GetCellCenter(b.Get<PositionComponent>())));

            //Iterate all collected static bodies for detail test
            foreach (var entity in entitySet)
                TestNarrowPhaseStatic(pack, entity, dt);
        }

        private int Xcomparison(DynamicPack x, DynamicPack y)
        {
            var xAabb = x.Aabb;
            var yAabb = y.Aabb;

            if (xAabb.Left < yAabb.Left)
                return -1;
            if (xAabb.Left == yAabb.Left)
                return 0;
            else
                return 1;
        }

        private int ShortestDistanceComparer(Vector2 pos, Vector2 a, Vector2 b)
        {
            var lx = Vector2.Distance(pos, a);
            var ly = Vector2.Distance(pos, b);

            if (lx < ly)
                return -1;
            if (lx == ly)
                return 0;
            else
                return 1;
        }

        private void InsertToGrid(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var body = entity.Get<BodyComponent>();

            UpdateAabb(body, pos);

            broadphaseMan.InsertStatic(gridId, entity.Id, body.Aabb);
        }

        private void RemoveFromGrid(Entity entity)
        {
            var aabb = GetAabb(entity);
            broadphaseMan.RemoveStatic(gridId, entity.Id, aabb);
        }

        private void RegisterStaticEntity(Entity entity)
        {
            InsertToGrid(entity);
        }

        private void UnregisterStaticEntity(Entity entity)
        {
            RemoveFromGrid(entity);
        }

        private void RegisterDynamicEntity(Entity entity)
        {
            var pack = new DynamicPack(entity.Id,
                                      entity.Get<BodyComponent>(),
                                      entity.Get<PositionComponent>(),
                                      entity.Get<VelocityComponent>());

            activeDynamics.Add(pack);
            UpdateAabb(pack.Body, pack.Position);
        }

        private void UnregisterDynamicEntity(Entity entity)
        {
            var dynamic = activeDynamics.FirstOrDefault(item => item.EntityId == entity.Id);

            if (dynamic == null)
                dynamic = inactiveDynamics.FirstOrDefault(item => item.EntityId == entity.Id);

            if (dynamic == null)
                throw new InvalidOperationException("Entity not found in this system.");

            activeDynamics.Remove(dynamic);
        }

        #endregion Private Methods
    }
}