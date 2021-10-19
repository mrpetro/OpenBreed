using OpenBreed.Wecs.Entities;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Physics.Interface.Managers
{
    public delegate void FixtureContactCallback(BodyFixture fixtureA, Entity entityA, BodyFixture fixtureB, Entity entityB, Vector2 projection);

    public delegate void CollisionPairCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection);

    public interface ICollisionMan
    {
        #region Public Methods

        void RegisterFixturePair(int fixtureIdA, int fixtureIdB, FixtureContactCallback callback);

        int RegisterGroup(string groupName);

        int GetGroupId(string item);

        int GetByName(string name);

        void Resolve(Entity entityA, Entity entityB, List<CollisionContact> contacts);

        #endregion Public Methods
    }

    public class BodyFixture
    {
        #region Public Constructors

        public BodyFixture(int shapeId, IEnumerable<int> groupIds)
        {
            ShapeId = shapeId;
            GroupIds = groupIds.ToList();
        }

        #endregion Public Constructors

        #region Public Properties

        public int ShapeId { get; set; }
        public List<int> GroupIds { get; }

        #endregion Public Properties
    }

    public class CollisionContact
    {
        #region Public Constructors

        public CollisionContact(BodyFixture fixtureA, BodyFixture fixtureB, Vector2 projection)
        {
            FixtureA = fixtureA;
            FixtureB = fixtureB;
            Projection = projection;
        }

        #endregion Public Constructors

        #region Public Properties

        public BodyFixture FixtureA { get; }
        public BodyFixture FixtureB { get; }
        public Vector2 Projection { get; }

        #endregion Public Properties
    }
}