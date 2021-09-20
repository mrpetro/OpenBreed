using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DynamicBodiesCollisionCheckSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;

        private IBroadphaseGrid broadphaseGrid;

        private readonly List<DynamicPack> inactiveDynamics = new List<DynamicPack>();
        private readonly List<DynamicPack> activeDynamics = new List<DynamicPack>();
        private readonly IEntityMan entityMan;
        private readonly IFixtureMan fixtureMan;
        private readonly ICollisionMan collisionMan;
        private readonly DynamicHelper dynamicHelper;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesCollisionCheckSystem(IEntityMan entityMan, IFixtureMan fixtureMan, ICollisionMan collisionMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
            this.collisionMan = collisionMan;
            this.dynamicHelper = new DynamicHelper(this, entityMan);

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<VelocityComponent>();
            RequireEntityWith<PositionComponent>();

            RegisterHandler<BodyOnCommand>(HandleBodyOnCommand);
            RegisterHandler<BodyOffCommand>(HandleBodyOffCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseGrid = world.GetModule<IBroadphaseGrid>();
        }

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
            var pack = new DynamicPack(entity.Id,
                                      entity.Get<BodyComponent>(),
                                      entity.Get<PositionComponent>(),
                                      entity.Get<VelocityComponent>());

            activeDynamics.Add(pack);
            UpdateAabb(pack.Body, pack.Position);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var dynamic = activeDynamics.FirstOrDefault(item => item.EntityId == entity.Id);

            if (dynamic == null)
                dynamic = inactiveDynamics.FirstOrDefault(item => item.EntityId == entity.Id);

            if (dynamic == null)
                throw new InvalidOperationException("Entity not found in this system.");

            activeDynamics.Remove(dynamic);
        }

        #endregion Protected Methods

        #region Private Methods

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

            return false;
        }

        private void UpdateAabb(BodyComponent body, PositionComponent pos)
        {
            var fixture = fixtureMan.GetById(body.Fixtures.First());
            body.Aabb = fixture.Shape.GetAabb().Translated(pos.Value);
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

            var idSet = broadphaseGrid.QueryStatic(dynamicAabb);

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

        #endregion Private Methods
    }
}