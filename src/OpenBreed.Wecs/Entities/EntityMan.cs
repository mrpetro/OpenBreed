using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBreed.Wecs.Entities
{
    public class EntityMan : IEntityMan
    {
        #region Private Fields

        private readonly IdMap<Entity> entities = new IdMap<Entity>();
        private readonly ICommandsMan commandsMan;
        private readonly IEventsMan eventsMan;


        #endregion Private Fields

        #region Internal Constructors

        internal EntityMan(ICommandsMan commandsMan, IEventsMan eventsMan)
        {
            this.commandsMan = commandsMan;
            this.eventsMan = eventsMan;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Count => entities.Count;

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
            var newEntity = new Entity(this, initialComponents);
            newEntity.Id = entities.Add(newEntity);
            return newEntity;
        }

        public void Destroy(Entity entity)
        {
            //worldMan.Subscribe<EntityRemovedEventArgs>(OnEntityRemovedEventArgs);
            commandsMan.Post(new RemoveEntityCommand(entity.Id, entity.Id));
        }

        public void Raise<T>(Entity entity, T eventArgs) where T : EventArgs
        {
            NotifyListeners(entity, eventArgs.GetType(), eventArgs);

            eventsMan.Raise<T>(entity, eventArgs);
        }


        private void NotifyListeners(object sender, Type eventType, EventArgs eventArgs)
        {
            List<(object Target, MethodInfo Method)> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            for (int i = 0; i < callbacks.Count; i++)
            {
                var item = callbacks[i];
                item.Method.Invoke(item.Target, new object[] { sender, eventArgs });
            }
        }

        private readonly Dictionary<Type, List<(object, MethodInfo)>> eventTypes = new Dictionary<Type, List<(object, MethodInfo)>>();

        public void Subscribe<T>(Entity entity, Action<object, T> callback) where T : EventArgs
        {


            eventsMan.Subscribe<T>(entity, callback);
        }

        public void Unsubscribe<T>(Entity entity, Action<object, T> callback) where T : EventArgs
        {
            eventsMan.Unsubscribe<T>(entity, callback);
        }

        #endregion Public Methods
    }
}