using OpenBreed.Game.Common;
using OpenBreed.Game.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Game.Entities
{
    public class EntityMan
    {
        #region Private Fields

        private readonly Dictionary<Guid, IEntity> entities = new Dictionary<Guid, IEntity>();
        private readonly List<IComponentSystem> systems = new List<IComponentSystem>();

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

        public void RegisterSystem(IComponentSystem system)
        {
            systems.Add(system);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddEntity(EntityBase entity)
        {
            entities.Add(entity.Guid, entity);

            entity.Initialize();
        }

        internal Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        #endregion Internal Methods

        #region Private Methods

        internal void InitializeComponent(IEntityComponent component)
        {
            var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

            if (foundSystem == null)
                throw new InvalidOperationException($"System {component.SystemType} not registered.");

            foundSystem.AddComponent(component);
        }

        #endregion Private Methods
    }
}