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

        private IBroadphaseStatic broadphaseGrid;
        private IBroadphaseDynamic broadphaseDynamic;

        private readonly List<Entity> inactiveDynamics = new List<Entity>();
        //private readonly List<DynamicPack> activeDynamics = new List<DynamicPack>();
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

            broadphaseGrid = world.GetModule<IBroadphaseStatic>();
            broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();
        }

        public static Vector2 GetCellCenter(PositionComponent pos)
        {
            return new Vector2(pos.Value.X + CELL_SIZE / 2, pos.Value.Y + CELL_SIZE / 2);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();
        }

        public void Update(float dt)
        {
            ExecuteCommands();

            broadphaseDynamic.Solve(QueryStaticGrid, TestNarrowPhaseDynamic, dt);
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

        }

        protected override void OnRemoveEntity(Entity entity)
        {

        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 GetAabb(Entity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var fixture = fixtureMan.GetById(body.Fixtures.First());
            return fixture.Shape.GetAabb().Translated(pos.Value);
        }

        private bool HandleBodyOnCommand(BodyOnCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            if (!entity.Contains<VelocityComponent>())
                return false;

            var dynamicToActivate = inactiveDynamics.FirstOrDefault(item => item == entity);

            if (dynamicToActivate != null)
            {
                var aabb = GetAabb(entity);
                broadphaseDynamic.InsertItem(entity.Id, aabb);
                inactiveDynamics.Remove(dynamicToActivate);
                entity.RaiseEvent(new BodyOnEventArgs(entity));
                return true;
            }

            return false;
        }

        private bool HandleBodyOffCommand(BodyOffCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            if (!entity.Contains<VelocityComponent>())
                return false;

            broadphaseDynamic.RemoveItem(entity.Id);

            //var dynamicToDeactivate = activeDynamics.FirstOrDefault(item => item.Entity == entity);

            //if (dynamicToDeactivate != null)
            //{


                inactiveDynamics.Add(entity);
                //activeDynamics.Remove(dynamicToDeactivate);

                entity.RaiseEvent(new BodyOffEventArgs(entity));
                return true;
            //}

            //return false;
        }

        private void TestNarrowPhaseDynamic(BroadphaseDynamicCell nextCollider, BroadphaseDynamicCell currentCollider, float dt)
        {
            var entityA = entityMan.GetById(nextCollider.ItemId);
            var entityB = entityMan.GetById(currentCollider.ItemId);

            Vector2 projection;
            if (dynamicHelper.TestVsDynamic(entityA, entityB, dt, out projection))
            {
                collisionMan.Callback(entityA, entityB, projection);

                //bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                //bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                //DynamicHelper.ResolveVsDynamic(bodyA, bodyB, projection, dt);
            }
        }

        private void TestNarrowPhaseStatic(Entity dynamicEntity, Entity staticEntity, float dt)
        {
            Vector2 projection;
            if (dynamicHelper.TestVsStatic(dynamicEntity, staticEntity, dt, out projection))
            {
                //var collisionDynamic = dynamicEntity.TryGet<CollisionComponent>();
                //if (collisionDynamic == null)
                //{
                //    collisionDynamic = new CollisionComponent();
                //    dynamicEntity.Add(collisionDynamic);
                //}

                //var collisionStatic = staticEntity.TryGet<CollisionComponent>();
                //if (collisionStatic == null)
                //{
                //    collisionStatic = new CollisionComponent();
                //    staticEntity.Add(collisionStatic);
                //}

                //collisionDynamic.Contacts.Add(new CollisionContact(staticEntity.Id, projection));
                //collisionStatic.Contacts.Add(new CollisionContact(dynamicEntity.Id, -projection));

                collisionMan.Callback(dynamicEntity, staticEntity, projection);
                collisionMan.Callback(staticEntity, dynamicEntity, -projection);
            }
        }

        private void QueryStaticGrid(BroadphaseDynamicCell cell, float dt)
        {
            var dynamicAabb = cell.Aabb;

            var idSet = broadphaseGrid.QueryStatic(dynamicAabb);

            if (idSet.Count == 0)
                return;

            var entity = entityMan.GetById(cell.ItemId);

            var entitySet = idSet.Select(id => entityMan.GetById(id)).ToList();

            var pos = dynamicAabb.GetCenter();

            entitySet.Sort((a, b) => ShortestDistanceComparer(pos, GetCellCenter(a.Get<PositionComponent>()), GetCellCenter(b.Get<PositionComponent>())));

            //Iterate all collected static bodies for detail test
            foreach (var staticEntity in entitySet)
                TestNarrowPhaseStatic(entity, staticEntity, dt);
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