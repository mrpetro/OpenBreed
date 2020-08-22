using OpenBreed.Core.Entities;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    public delegate void CollisionPairCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection);

    internal class ColliderType
    {
        #region Private Fields

        private Dictionary<int, CollisionPairCallback> callbacks = new Dictionary<int, CollisionPairCallback>();

        #endregion Private Fields

        #region Internal Constructors

        internal ColliderType(string name)
        {
            Name = name;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal string Name { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal void Callback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            if (callbacks.TryGetValue(colliderTypeB, out CollisionPairCallback callback))
                callback.Invoke(colliderTypeA, entityA, colliderTypeB, entityB, projection);
        }

        internal bool Any => callbacks.Any();

        internal void UnregisterCallback(int colliderTypeB)
        {
            callbacks.Remove(colliderTypeB);
        }

        internal void RegisterCallback(int colliderTypeB, CollisionPairCallback callback)
        {
            callbacks.Add(colliderTypeB, callback);
        }

        #endregion Internal Methods
    }
}