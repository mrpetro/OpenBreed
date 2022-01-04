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
        private readonly List<Entity> inactiveDynamics = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IShapeMan shapeMan;
        private readonly ICollisionMan<Entity> collisionMan;
        private IBroadphaseStatic broadphaseGrid;
        private IBroadphaseDynamic broadphaseDynamic;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesCollisionCheckSystem(IEntityMan entityMan, IShapeMan shapeMan, ICollisionMan<Entity> collisionMan)
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

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            broadphaseDynamic.Solve(QueryStaticGrid, TestNarrowPhaseDynamic, dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => broadphaseDynamic.ContainsItem(entity.Id);

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
            var shape = shapeMan.GetById(body.Fixtures.First().ShapeId);
            return shape.GetAabb().Translated(pos.Value);
        }

        //private bool HandleBodyOnCommand(BodyOnCommand cmd)
        //{
        //    var entity = entityMan.GetById(cmd.EntityId);

        //    if (!entity.Contains<VelocityComponent>())
        //        return false;

        //    var dynamicToActivate = inactiveDynamics.FirstOrDefault(item => item == entity);

        //    if (dynamicToActivate != null)
        //    {
        //        var aabb = GetAabb(entity);
        //        broadphaseDynamic.InsertItem(entity.Id, aabb);
        //        inactiveDynamics.Remove(dynamicToActivate);
        //        entity.RaiseEvent(new BodyOnEventArgs(entity));
        //        return true;
        //    }

        //    return false;
        //}

        //private bool HandleBodyOffCommand(BodyOffCommand cmd)
        //{
        //    var entity = entityMan.GetById(cmd.EntityId);

        //    if (!entity.Contains<VelocityComponent>())
        //        return false;

        //    broadphaseDynamic.RemoveItem(entity.Id);

        //    inactiveDynamics.Add(entity);

        //    entity.RaiseEvent(new BodyOffEventArgs(entity));
        //    return true;
        //}

        private void TestNarrowPhaseDynamic(BroadphaseDynamicElement nextCollider, BroadphaseDynamicElement currentCollider, float dt)
        {
            var entityA = entityMan.GetById(nextCollider.ItemId);
            var entityB = entityMan.GetById(currentCollider.ItemId);

            if (TestVsDynamic(entityA, entityB, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
                collisionMan.Resolve(entityA, entityB, contacts);
        }

        private void TestNarrowPhaseStatic(Entity dynamicEntity, Entity staticEntity, float dt)
        {
            if (TestVsStatic(dynamicEntity, staticEntity, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
                collisionMan.Resolve(dynamicEntity, staticEntity, contacts);
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

        private bool TestVsDynamic(Entity entityA, Entity entityB, float dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts)
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
                var shapeA = shapeMan.GetById(fixtureA.ShapeId);

                foreach (var fixtureB in bodyB.Fixtures)
                {
                    var shapeB = shapeMan.GetById(fixtureB.ShapeId);

                    if (CollisionChecker.Check(posA.Value, shapeA, posB.Value, shapeB, out Vector2 projection))
                        contacts.Add(new OpenBreed.Physics.Interface.Managers.CollisionContact(fixtureA, fixtureB, projection));
                }
            }

            return contacts.Count > 0;
        }

        private bool TestVsStatic(Entity dynamicEntity, Entity staticEntity, float dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts)
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
                var shapeA = shapeMan.GetById(fixtureA.ShapeId);

                foreach (var fixtureB in bodyB.Fixtures)
                {
                    var shapeB = shapeMan.GetById(fixtureB.ShapeId);

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