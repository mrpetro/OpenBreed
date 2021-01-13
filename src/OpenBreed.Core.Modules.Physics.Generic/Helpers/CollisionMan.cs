using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Components.Physics;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Physics.Helpers
{
    internal class CollisionMan : ICollisionMan
    {
        #region Private Fields

        private readonly Dictionary<string, int> aliases = new Dictionary<string, int>();
        private readonly ILogger logger;
        private IdMap<ColliderType> list = new IdMap<ColliderType>();

        #endregion Private Fields

        #region Public Constructors

        public CollisionMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterCollisionPair(int colliderTypeA, int colliderTypeB, CollisionPairCallback callback)
        {
            list[colliderTypeA].RegisterCallback(colliderTypeB, callback);
            //list[colliderTypeB].RegisterCallback(colliderTypeA, callback);
        }

        public void UnregisterCollisionPair(int colliderTypeA, int colliderTypeB)
        {
            var callbacks = list[colliderTypeA];
            callbacks.UnregisterCallback(colliderTypeB);
            list.RemoveById(colliderTypeA);
        }

        public int CreateColliderType(string colliderTypeName)
        {
            var newColliderType = new ColliderType(colliderTypeName);
            var id = list.Add(newColliderType);
            aliases.Add(colliderTypeName, id);
            return id;
        }

        public int GetByName(string name)
        {
            int result;
            aliases.TryGetValue(name, out result);
            return result;
        }

        public string GetNameById(int id)
        {
            return list[id].Name;
        }

        public void Callback(Entity entityA, Entity entityB, Vector2 projection)
        {
            var colCmpA = entityA.Get<CollisionComponent>();
            var colCmpB = entityB.Get<CollisionComponent>();

            for (int i = 0; i < colCmpA.ColliderTypes.Count; i++)
            {
                var colliderTypeId = colCmpA.ColliderTypes[i];
                var colliderType = list[colliderTypeId];
                for (int j = 0; j < colCmpB.ColliderTypes.Count; j++)
                    colliderType.Callback(colliderTypeId, entityA, colCmpB.ColliderTypes[j], entityB, projection);
            }

            //for (int i = 0; i < colCmpB.ColliderTypes.Count; i++)
            //{
            //    var colliderTypeId = colCmpB.ColliderTypes[i];
            //    for (int j = 0; j < colCmpA.ColliderTypes.Count; j++)
            //        list[colliderTypeId].Callback(colliderTypeId, entityB, colCmpA.ColliderTypes[i], entityA, projection);
            //}
        }

        #endregion Public Methods
    }
}