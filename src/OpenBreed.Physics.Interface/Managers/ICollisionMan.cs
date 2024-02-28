using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Physics.Interface.Managers
{
    public delegate void FixtureContactCallback<TObject>(IFixture fixtureA, TObject objA, IFixture fixtureB, TObject objB, float dt, Vector2 projection);

    public interface ICollisionMan<TObject>
    {
        #region Public Properties

        IEnumerable<string> GroupNames { get; }

        #endregion Public Properties

        #region Public Methods

        int GetByName(string name);

        int GetGroupId(string item);

        void RegisterFixturePair(int fixtureIdA, int fixtureIdB, FixtureContactCallback<TObject> callback);

        int RegisterGroup(string groupName);

        void Resolve(TObject objA, TObject objB, float dt, List<CollisionContact> contacts);

        #endregion Public Methods
    }

    public class BodyFixture : IFixture
    {
        #region Public Constructors

        public BodyFixture(
            int id,
            IShape shape,
            IEnumerable<int> groupIds)
        {
            Id = id;
            Shape = shape;
            GroupIds = groupIds.ToList();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<int> GroupIds { get; }
        public int Id { get; }
        public IShape Shape { get; set; }

        #endregion Public Properties
    }

    public class CollisionContact : IEquatable<CollisionContact>
    {
        #region Public Constructors

        public CollisionContact(
            int itemIdA,
            int itemIdB,
            IFixture fixtureA,
            IFixture fixtureB,
            Vector2 projection)
        {
            ItemIdA = itemIdA;
            ItemIdB = itemIdB;
            FixtureA = fixtureA;
            FixtureB = fixtureB;
            Projection = projection;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ItemIdA { get; }
        public int ItemIdB { get; }
        public IFixture FixtureA { get; }
        public IFixture FixtureB { get; }
        public Vector2 Projection { get; }

        public CollisionContact Copy()
        {
            return new CollisionContact(ItemIdA, ItemIdB, FixtureA, FixtureB, Projection);
        }

        public override int GetHashCode()
        {
            return ItemIdA ^ ItemIdB ^ FixtureA.GetHashCode() ^ FixtureB.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not CollisionContact other)
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(CollisionContact other)
        {
            if (other is null)
            {
                return false;
            }

            return ItemIdA == other.ItemIdA
                && FixtureA == other.FixtureA
                && ItemIdB == other.ItemIdB
                && FixtureB == other.FixtureB;
        }

        #endregion Public Properties
    }
}