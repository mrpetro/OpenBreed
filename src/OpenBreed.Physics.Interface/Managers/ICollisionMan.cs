using OpenBreed.Wecs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Physics.Interface.Managers
{
    public delegate void CollisionPairCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection);

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

        #endregion Public Methods
    }
}