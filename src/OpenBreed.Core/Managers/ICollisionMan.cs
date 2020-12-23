using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Managers
{
    public delegate void CollisionPairCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection);

    public interface ICollisionMan
    {
        #region Internal Properties

        #endregion Internal Properties

        #region Public Methods

        public void RegisterCollisionPair(int colliderTypeA, int colliderTypeB, CollisionPairCallback callback);

        public void UnregisterCollisionPair(int colliderTypeA, int colliderTypeB);

        public int CreateColliderType(string colliderTypeName);

        public int GetByName(string name);

        public string GetNameById(int id);

        void Callback(Entity entityA, Entity entityB, Vector2 projection);

        #endregion Public Methods
    }
}