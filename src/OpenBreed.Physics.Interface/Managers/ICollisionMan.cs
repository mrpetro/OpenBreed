using OpenBreed.Wecs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Physics.Interface.Managers
{
    public delegate void CollisionPairCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection);

    public class CollisionManifold
    {
        public CollisionManifold(int fixtureIdA, int fixtureIdB, Vector2 projection)
        {
            FixtureIdA = fixtureIdA;
            FixtureIdB = fixtureIdB;
            Projection = projection;
        }

        public int FixtureIdA { get; }
        public int FixtureIdB { get; }
        public Vector2 Projection { get; }
    }

    public interface ICollisionMan
    {
        #region Internal Properties

        #endregion Internal Properties

        #region Public Methods

        void RegisterCollisionPair(int colliderTypeA, int colliderTypeB, CollisionPairCallback callback);

        void UnregisterCollisionPair(int colliderTypeA, int colliderTypeB);

        int CreateColliderType(string colliderTypeName);

        int GetByName(string name);

        string GetNameById(int id);

        void Callback(Entity entityA, Entity entityB, Vector2 projection);

        void Resolve(Entity entityA, Entity entityB, List<CollisionManifold> manifolds);

        #endregion Public Methods
    }
}