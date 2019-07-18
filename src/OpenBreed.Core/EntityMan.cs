using OpenBreed.Core.Collections;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core
{
    public class EntityMan
    {
        #region Private Fields

        private readonly IdMap<IEntity> entities = new IdMap<IEntity>();

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

        public IEntity GetById(int id)
        {
            var entity = entities[id];

            if (entities.TryGetValue(id, out entity))
                return entity;
            else
                throw new InvalidOperationException($"Entity with Guid '{id}' not found.");
        }

        public IEntity Create()
        {
            var newEntity = new Entity(Core);
            newEntity.Id = entities.Add(newEntity);
            return newEntity;
        }

        #endregion Public Methods
    }
}