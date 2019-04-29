using OpenBreed.Game.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game
{
    public class EntityMan
    {
        #region Private Fields

        private readonly Dictionary<Guid, IEntity> entities = new Dictionary<Guid, IEntity>();

        #endregion Private Fields

        #region Public Methods

        public IEntity GetByGuid(Guid guid)
        {
            IEntity entity;

            if (entities.TryGetValue(guid, out entity))
                return entity;
            else
                throw new InvalidOperationException($"Entity with Guid '{guid}' not found.");
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddEntity(EntityBase entity)
        {
            entities.Add(entity.Guid, entity);
        }

        internal Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        #endregion Internal Methods
    }
}