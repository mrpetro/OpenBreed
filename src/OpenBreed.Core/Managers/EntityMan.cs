using NLua;
using OpenBreed.Core.Collections;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Builders;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    internal class EntityMan : IEntityMan
    {
        #region Private Fields

        private readonly IdMap<Entity> entities = new IdMap<Entity>();
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        internal EntityMan(ICore core, ICommandsMan commandsMan)
        {
            Core = core;
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<Entity> GetByTag(object tag)
        {
            return entities.Items.Where(item => item.Tag != null && item.Tag.Equals(tag));
        }

        public IEnumerable<Entity> Where(Func<Entity, bool> predicate)
        {
            return entities.Items.Where(predicate);
        }

        public Entity GetById(int id)
        {
            if (entities.TryGetValue(id, out Entity entity))
                return entity;
            else
                return null;
        }

        public Entity Create(List<IEntityComponent> initialComponents = null)
        {
            var newEntity = new Entity(Core, initialComponents);
            newEntity.Id = entities.Add(newEntity);
            return newEntity;
        }

        public void Destroy(Entity entity)
        {
            Core.Worlds.Subscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            entity.Core.Commands.Post(new RemoveEntityCommand(entity.World.Id, entity.Id));
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEntityRemovedEventArgs(object sender, EntityRemovedEventArgs e)
        {
            var entity = Core.Entities.GetById(e.EntityId);
            Core.Worlds.Unsubscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            entities.RemoveById(entity.Id);
        }

        #endregion Private Methods
    }
}