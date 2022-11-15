using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Entities
{
    internal class EntityMan : IEntityMan
    {
        #region Private Fields

        private readonly IdMap<Entity> entities = new IdMap<Entity>();
        private readonly IEventsMan eventsMan;
        private readonly HashSet<Entity> toDestory = new HashSet<Entity>();

        private readonly HashSet<Entity> entityByNoTagLookup = new HashSet<Entity>();
        private readonly Dictionary<string, HashSet<Entity>> entityByTagLookup = new Dictionary<string, HashSet<Entity>>();

        #endregion Private Fields

        #region Internal Constructors

        public EntityMan(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan;

            this.eventsMan.Subscribe<EntityLeftEventArgs>(OnEntityLeftWorld);
        }

        #endregion Internal Constructors

        #region Public Events

        public event ComponentAdded ComponentAdded;

        public event ComponentRemoved ComponentRemoved;

        public event EnteringWorld EnterWorldRequested;

        public event LeavingWorld LeaveWorldRequested;

        #endregion Public Events

        #region Public Properties

        public int Count => entities.Count;

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<Entity> GetByTag(string tag)
        {
            if (tag is null)
                return entityByNoTagLookup;

            if (entityByTagLookup.TryGetValue(tag, out HashSet<Entity> found))
                return found;

            return Enumerable.Empty<Entity>();
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

        public Entity Create(string tag, List<IEntityComponent> initialComponents = null)
        {
            var newEntity = new Entity(this, tag, initialComponents);
            newEntity.Id = entities.Add(newEntity);

            AddToLookup(tag, newEntity);

            return newEntity;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void RequestLeaveWorld(Entity entity)
        {
            LeaveWorldRequested?.Invoke(entity);
        }

        internal void RequestEnterWorld(Entity entity, int worldId)
        {
            EnterWorldRequested?.Invoke(entity, worldId);
        }

        internal void RequestDestroy(Entity entity)
        {
            toDestory.Add(entity);
        }

        internal void OnComponentAdded(Entity entity, Type componentType)
        {
            ComponentAdded?.Invoke(entity, componentType);
        }

        internal void OnComponentRemoved(Entity entity, Type componentType)
        {
            ComponentRemoved?.Invoke(entity, componentType);
        }

        internal void Raise<T>(Entity entity, T eventArgs) where T : EventArgs
        {
            eventsMan.Raise<T>(entity, eventArgs);
        }

        #endregion Internal Methods

        #region Private Methods

        private void OnEntityLeftWorld(object sender, EntityLeftEventArgs e)
        {
            var entity = (Entity)sender;

            if (toDestory.Contains(entity))
            {
                entities.RemoveById(entity.Id);
                RemoveFromLookup(entity.Tag, entity);
                toDestory.Remove(entity);
            }
        }

        private void AddToLookup(string tag, Entity entity)
        {
            if(tag is null)
            {
                entityByNoTagLookup.Add(entity);
                return;
            }

            if (!entityByTagLookup.TryGetValue(tag, out HashSet<Entity> foundEntities))
            {
                foundEntities = new HashSet<Entity>();
                entityByTagLookup.Add(tag, foundEntities);
            }

            foundEntities.Add(entity);
        }

        private void RemoveFromLookup(string tag, Entity entity)
        {
            if (tag is null)
            {
                entityByNoTagLookup.Remove(entity);
                return;
            }

            if (!entityByTagLookup.TryGetValue(tag, out HashSet<Entity> foundEntities))
                return;

            foundEntities.Remove(entity);

            if (foundEntities.Any())
                return;

            entityByTagLookup.Remove(tag);
        }

        #endregion Private Methods
    }
}