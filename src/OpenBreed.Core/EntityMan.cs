using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core
{
    public class EntityMan
    {
        public ICore Core { get; }

        public EntityMan(ICore core)
        {
            Core = core;
        }

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

        internal void AddEntity(IEntity entity)
        {
            entities.Add(entity.Guid, entity);
        }

        internal Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public IEntity Create()
        {
            return new Entity(Core);
        }

        #endregion Internal Methods
    }
}