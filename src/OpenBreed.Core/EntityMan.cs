using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core
{
    public class EntityMan
    {
        #region Private Fields

        private readonly Dictionary<Guid, IEntity> entities = new Dictionary<Guid, IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public EntityMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public IEntity GetByGuid(Guid guid)
        {
            IEntity entity;

            if (entities.TryGetValue(guid, out entity))
                return entity;
            else
                throw new InvalidOperationException($"Entity with Guid '{guid}' not found.");
        }

        public IEntity Create()
        {
            return new Entity(this);
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

        internal void PostMsg(Entity sender, IEntityMsg message)
        {
            if (sender.CurrentWorld == null)
                return;

            foreach (var system in sender.CurrentWorld.Systems)
            {
                if (system.HandleMsg(sender, message))
                    break;
            }
        }

        #endregion Internal Methods
    }
}