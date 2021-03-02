using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Entities
{
    public class EntityMan : IEntityMan
    {
        #region Private Fields

        private readonly IdMap<Entity> entities = new IdMap<Entity>();
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal EntityMan(ICore core, ICommandsMan commandsMan)
        {
            Core = core;
            this.commandsMan = commandsMan;
        }

        #endregion Internal Constructors

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
            //worldMan.Subscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            commandsMan.Post(new RemoveEntityCommand(entity.World.Id, entity.Id));
        }

        #endregion Public Methods

        #region Private Methods

        //private void OnEntityRemovedEventArgs(object sender, EntityRemovedEventArgs e)
        //{
        //    var entity = GetById(e.EntityId);
        //    worldMan.Unsubscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
        //    entities.RemoveById(entity.Id);
        //}

        #endregion Private Methods
    }
}