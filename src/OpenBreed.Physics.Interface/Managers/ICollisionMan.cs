using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Physics.Interface.Managers
{
    public delegate void FixtureContactCallback<TObject>(BodyFixture fixtureA, TObject objA, BodyFixture fixtureB, TObject objB, Vector2 projection);

    public interface ICollisionMan<TObject>
    {
        #region Public Methods

        int GetByName(string name);

        int GetGroupId(string item);

        void RegisterFixturePair(int fixtureIdA, int fixtureIdB, FixtureContactCallback<TObject> callback);

        int RegisterGroup(string groupName);

        void Resolve(TObject objA, TObject objB, List<CollisionContact> contacts);

        #endregion Public Methods
    }

    public class BodyFixture : IFixture
    {
        #region Internal Constructors

        public BodyFixture(
            int id,
            IShape shape,
            IEnumerable<int> groupIds)
        {
            Id = id;
            Shape = shape;
            GroupIds = groupIds.ToList();
        }

        #endregion Internal Constructors

        #region Public Properties

        public List<int> GroupIds { get; }
        public int Id { get; }
        public IShape Shape { get; set; }

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