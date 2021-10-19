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
        private readonly List<Entity> inactiveDynamics = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IFixtureMan fixtureMan;
        private readonly ICollisionMan collisionMan;
        private IBroadphaseStatic broadphaseGrid;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesCollisionCheckSystem(IEntityMan entityMan, IFixtureMan fixtureMan, ICollisionMan collisionMan)
        {
            this.entityMan = entityMan;
            this.fixtureMan = fixtureMan;
            this.collisionMan = collisionMan;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<VelocityComponent>();
            RequireEntityWith<PositionComponent>();

            RegisterHandler<BodyOnCommand>(HandleBodyOnCommand);
            RegisterHandler<BodyOffCommand>(HandleBodyOffCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public static Vector2 GetCellCenter(PositionComponent pos)
        {
            return new Vector2(pos.Value.X + CELL_SIZE / 2, pos.Value.Y + CELL_SIZE / 2);
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);

            broadphaseGrid = world.GetModule<IBroadphaseStatic>();
            broadphaseDynamic = world.GetModule<IBroadphaseDynamic>();
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

            inactiveDynamics.Add(entity);

            entity.RaiseEvent(new BodyOffEventArgs(entity));
            return true;
        }

        private void TestNarrowPhaseDynamic(BroadphaseDynamicElement nextCollider, BroadphaseDynamicElement currentCollider, float dt)
        {
            var entityA = entityMan.GetById(nextCollider.ItemId);
            var entityB = entityMan.GetById(currentCollider.ItemId);

            Vector2 projection;
            if (TestVsDynamic(entityA, entityB, dt, out projection))
            {
                collisionMan.Callback(entityA, entityB, projection);

                //bodyA.Entity.RaiseEvent(new CollisionEvent(bodyB.Entity));
                //bodyB.Entity.RaiseEvent(new CollisionEvent(bodyA.Entity));
                //DynamicHelper.ResolveVsDynamic(bodyA, bodyB, projection, dt);
            }
        }

        private void TestNarrowPhaseStatic(Entity dynamicEntity, Entity staticEntity, float dt)
        {
            if (TestVsStatic(dynamicEntity, staticEntity, dt, out List<CollisionManifold> manifolds))
                collisionMan.Resolve(dynamicEntity, staticEntity, manifolds);
            //{
            //    var projection = manifolds.First().Projection;
            //    //var collisionDynamic = dynamicEntity.TryGet<CollisionComponent>();
            //    //if (collisionDynamic == null)
            //    //{
            //    //    collisionDynamic = new CollisionComponent();
            //    //    dynamicEntity.Add(collisionDynamic);
            //    //}

            //    //var collisionStatic = staticEntity.TryGet<CollisionComponent>();
            //    //if (collisionStatic == null)
            //    //{
            //    //    collisionStatic = new CollisionComponent();
            //    //    staticEntity.Add(collisionStatic);
            //    //}

            //    //collisionDynamic.Contacts.Add(new CollisionContact(staticEntity.Id, projection));
            //    //collisionStatic.Contacts.Add(new CollisionContact(dynamicEntity.Id, -projection));

            //    collisionMan.Callback(dynamicEntity, staticEntity, projection);
            //    //collisionMan.Callback(staticEntity, dynamicEntity, -projection);
            //}
        }

        private bool TestVsDynamic(Entity entityA, Entity entityB, float dt, out Vector2 projection)
        {
            var bodyA = entityA.Get<BodyComponent>();
            var posA = entityA.Get<PositionComponent>();
            var bodyB = entityB.Get<BodyComponent>();
            var posB = entityB.Get<PositionComponent>();

            if (bodyA.Fixtures.Count > 0)
            {
                var fixtureA = fixtureMan.GetById(bodyA.Fixtures.First());

                if (bodyB.Fixtures.Count > 0)
                {
                    var fixtureB = fixtureMan.GetById(bodyB.Fixtures.First());
                    return CollisionChecker.Check(posA.Value, fixtureA, posB.Value, fixtureB, out projection);
                }
                else
                    throw new NotImplementedException();
            }
            else
                throw new NotImplementedException();
        }

        private bool TestVsStatic(Entity dynamicEntity, Entity staticEntity, float dt, out List<CollisionManifold> manifolds)
        {
            var bodyA = dynamicEntity.Get<BodyComponent>();
            var posA = dynamicEntity.Get<PositionComponent>();

            if (bodyA.Fixtures.Count == 0)
            {
                manifolds = null;
                return false;
            }

            var bodyB = staticEntity.Get<BodyComponent>();
            var posB = staticEntity.Get<PositionComponent>();

            if (bodyB.Fixtures.Count == 0)
            {
                manifolds = null;
                return false;
            }

            manifolds = new List<CollisionManifold>();

            foreach (var fixtureAId in bodyA.Fixtures)
            {
                var fixtureA = fixtureMan.GetById(fixtureAId);

                foreach (var fixtureBId in bodyB.Fixtures)
                {
                    var fixtureB = fixtureMan.GetById(fixtureBId);

                    if (CollisionChecker.Check(posA.Value, fixtureA, posB.Value, fixtureB, out Vector2 projection))
                        manifolds.Add(new CollisionManifold(fixtureAId, fixtureBId, projection));
                }
            }

            return manifolds.Count > 0;
        }

        private void QueryStaticGrid(BroadphaseDynamicElement cell, float dt)
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