using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DynamicBodiesCollisionCheckSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;
        private readonly List<IEntity> inactiveDynamics = new List<IEntity>();
        private readonly IEntityMan entityMan;
        private readonly IShapeMan shapeMan;
        private readonly ICollisionMan<IEntity> collisionMan;
        private IBroadphaseStatic broadphaseGrid;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesCollisionCheckSystem(IEntityMan entityMan, IShapeMan shapeMan, ICollisionMan<IEntity> collisionMan)
        {
            this.entityMan = entityMan;
            this.shapeMan = shapeMan;
            this.collisionMan = collisionMan;

            RequireEntityWith<BodyComponent>();
            RequireEntityWith<VelocityComponent>();
            RequireEntityWith<PositionComponent>();

            //RegisterHandler<BodyOnCommand>(HandleBodyOnCommand);
            //RegisterHandler<BodyOffCommand>(HandleBodyOffCommand);
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

        public void Update(IWorldContext context)
        {
            broadphaseDynamic.Solve(QueryStaticGrid, TestNarrowPhaseDynamic, context.Dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(IEntity entity) => broadphaseDynamic.ContainsItem(entity.Id);

        protected override void OnAddEntity(IEntity entity)
        {
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 GetAabb(IEntity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var shape = body.Fixtures.First().Shape;
            return shape.GetAabb().Translated(pos.Value);
        }

        private void TestNarrowPhaseDynamic(BroadphaseDynamicElement nextCollider, BroadphaseDynamicElement currentCollider, float dt)
        {
            var entityA = entityMan.GetById(nextCollider.ItemId);
            var entityB = entityMan.GetById(currentCollider.ItemId);

            if (TestVsDynamic(entityA, entityB, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
                collisionMan.Resolve(entityA, entityB, dt, contacts);
        }

        private void TestNarrowPhaseStatic(IEntity dynamicEntity, IEntity staticEntity, float dt)
        {
            if (TestVsStatic(dynamicEntity, staticEntity, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
                collisionMan.Resolve(dynamicEntity, staticEntity, dt, contacts);
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

        private bool TestVsDynamic(IEntity entityA, IEntity entityB, float dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts)
        {
            var bodyA = entityA.Get<BodyComponent>();
            var posA = entityA.Get<PositionComponent>();

            if (bodyA.Fixtures.Count == 0)
            {
                contacts = null;
                return false;
            }

            var bodyB = entityB.Get<BodyComponent>();
            var posB = entityB.Get<PositionComponent>();

            if (bodyB.Fixtures.Count == 0)
            {
                contacts = null;
                return false;
            }

            contacts = new List<OpenBreed.Physics.Interface.Managers.CollisionContact>();

            foreach (var fixtureA in bodyA.Fixtures)
            {
                var shapeA = fixtureA.Shape;

                foreach (var fixtureB in bodyB.Fixtures)
                {
                    var shapeB = fixtureB.Shape;

                    if (CollisionChecker.Check(posA.Value, shapeA, posB.Value, shapeB, out Vector2 projection))
                        contacts.Add(new OpenBreed.Physics.Interface.Managers.CollisionContact(fixtureA, fixtureB, projection));
                }
            }

            return contacts.Count > 0;
        }

        private bool TestVsStatic(IEntity dynamicEntity, IEntity staticEntity, float dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts)
        {
            var bodyA = dynamicEntity.Get<BodyComponent>();
            var posA = dynamicEntity.Get<PositionComponent>();

            if (bodyA.Fixtures.Count == 0)
            {
                contacts = null;
                return false;
            }

            var bodyB = staticEntity.Get<BodyComponent>();
            var posB = staticEntity.Get<PositionComponent>();

            if (bodyB.Fixtures.Count == 0)
            {
                contacts = null;
                return false;
            }

            contacts = new List<OpenBreed.Physics.Interface.Managers.CollisionContact>();

            foreach (var fixtureA in bodyA.Fixtures)
            {
                var shapeA = fixtureA.Shape;

                foreach (var fixtureB in bodyB.Fixtures)
                {
                    var shapeB = fixtureB.Shape;

                    if (CollisionChecker.Check(posA.Value, shapeA, posB.Value, shapeB, out Vector2 projection))
                        contacts.Add(new OpenBreed.Physics.Interface.Managers.CollisionContact(fixtureA, fixtureB, projection));
                }
            }

            return contacts.Count > 0;
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