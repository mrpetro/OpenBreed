using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DynamicBodiesCollisionCheckSystem : IUpdatableSystem
    {
        #region Private Fields

        private const int CELL_SIZE = 16;
        private readonly ICollisionChecker collisionChecker;
        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly IEntityMan entityMan;
        private readonly IShapeMan shapeMan;

        #endregion Private Fields

        #region Internal Constructors

        internal DynamicBodiesCollisionCheckSystem(
            IEntityMan entityMan,
            IShapeMan shapeMan,
            ICollisionMan<IEntity> collisionMan,
            ICollisionChecker collisionChecker)
        {
            this.entityMan = entityMan;
            this.shapeMan = shapeMan;
            this.collisionMan = collisionMan;
            this.collisionChecker = collisionChecker;
        }

        #endregion Internal Constructors

        #region Public Methods

        public static Vector2 GetCellCenter(PositionComponent pos)
        {
            return new Vector2(pos.Value.X + CELL_SIZE / 2, pos.Value.Y + CELL_SIZE / 2);
        }

        public void Update(IUpdateContext context)
        {
            var mapEntity = entityMan.GetByTag("Maps").Where(e => e.WorldId == context.World.Id).FirstOrDefault();

            if (mapEntity is null)
                return;

            var collisionComponent = mapEntity.Get<CollisionComponent>();

            collisionComponent.ContactPairs.Clear();
            collisionComponent.Broadphase.Solve(
                QueryStaticGrid,
                TestNarrowPhaseDynamic,
                collisionComponent.ContactPairs,
                context.Dt);
        }

        #endregion Public Methods

        #region Private Methods

        private Box2 GetAabb(IEntity entity)
        {
            var body = entity.Get<BodyComponent>();
            var pos = entity.Get<PositionComponent>();
            var shape = body.Fixtures.First().Shape;
            return shape.GetAabb().Translated(pos.Value);
        }

        private void QueryStaticGrid(IBroadphase grid, BroadphaseItem cell, List<ContactPair> contactPairs, float dt)
        {
            var dynamicAabb = cell.Aabb;

            var idSet = grid.QueryStatic(dynamicAabb);

            if (idSet.Count == 0)
            {
                return;
            }

            var entity = entityMan.GetById(cell.ItemId);

            var entitySet = idSet.Select(id => entityMan.GetById(id)).ToList();

            var pos = dynamicAabb.GetCenter();

            entitySet.Sort((a, b) => ShortestDistanceComparer(pos, GetCellCenter(a.Get<PositionComponent>()), GetCellCenter(b.Get<PositionComponent>())));

            //Iterate all collected static bodies for detail test
            foreach (var staticEntity in entitySet)
            {
                var contacts = TestNarrowPhaseStatic(entity, staticEntity, dt);

                if (contacts is not null)
                {
                    contactPairs.Add(new ContactPair(entity.Id, staticEntity.Id, contacts));
                }
            }
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

        private void TestNarrowPhaseDynamic(
                            BroadphaseItem nextCollider,
            BroadphaseItem currentCollider,
            List<ContactPair> result,
            float dt)
        {
            var entityA = entityMan.GetById(nextCollider.ItemId);
            var entityB = entityMan.GetById(currentCollider.ItemId);

            if (TestVsDynamic(entityA, entityB, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
            {
                result.Add(new ContactPair(nextCollider.ItemId, currentCollider.ItemId, contacts));
                collisionMan.Resolve(entityA, entityB, dt, contacts);
            }
        }

        private List<OpenBreed.Physics.Interface.Managers.CollisionContact> TestNarrowPhaseStatic(IEntity dynamicEntity, IEntity staticEntity, float dt)
        {
            if (TestVsStatic(dynamicEntity, staticEntity, dt, out List<OpenBreed.Physics.Interface.Managers.CollisionContact> contacts))
            {
                collisionMan.Resolve(dynamicEntity, staticEntity, dt, contacts);
                return contacts;
            }

            return null;
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

                    if (collisionChecker.Check(posA.Value, shapeA, posB.Value, shapeB, out Vector2 projection))
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

                    if (collisionChecker.Check(posA.Value, shapeA, posB.Value, shapeB, out Vector2 projection))
                        contacts.Add(new OpenBreed.Physics.Interface.Managers.CollisionContact(fixtureA, fixtureB, projection));
                }
            }

            return contacts.Count > 0;
        }

        #endregion Private Methods
    }
}