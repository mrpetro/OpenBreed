using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Entities
{
    internal class EntityMan : IEntityMan
    {
        #region Private Fields

        private readonly IdMap<IEntity> entities = new IdMap<IEntity>();
        private readonly HashSet<IEntity> entityByNoTagLookup = new HashSet<IEntity>();
        private readonly Dictionary<string, HashSet<IEntity>> entityByTagLookup = new Dictionary<string, HashSet<IEntity>>();
        private readonly IEventsMan eventsMan;
        private readonly HashSet<IEntity> toErase = new HashSet<IEntity>();

        #endregion Private Fields

        #region Public Constructors

        public EntityMan(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Events

        public event ComponentAdded ComponentAdded;

        public event ComponentRemoved ComponentRemoved;

        #endregion Public Events

        #region Public Properties

        public int Count => entities.Count;

        #endregion Public Properties

        #region Public Methods

        public IEntity Create(string tag, List<IEntityComponent> initialComponents = null)
        {
            var newEntity = new Entity(this, tag, initialComponents);
            newEntity.Id = entities.Add(newEntity);

            AddToLookup(tag, newEntity);

            return newEntity;
        }

        public IEntity GetById(int id)
        {
            if (entities.TryGetValue(id, out IEntity entity))
                return entity;
            else
                return null;
        }

        public IEnumerable<IEntity> GetByTag(string tag)
        {
            if (tag is null)
                return entityByNoTagLookup;

            if (entityByTagLookup.TryGetValue(tag, out HashSet<IEntity> found))
                return found;

            return Enumerable.Empty<IEntity>();
        }

        public void RequestErase(IEntity entity)
        {
            toErase.Add(entity);
        }

        public IEnumerable<IEntity> Where(Func<IEntity, bool> predicate)
        {
            return entities.Items.Where(predicate);
        }

        public void Cleanup()
        {
            foreach (var entity in toErase)
            {
                //Erase only entities that have no world 
                if (entity.HasWorld())
                {
                    continue;
                }

                entities.RemoveById(entity.Id);
                RemoveFromLookup(entity.Tag, entity);
            }

            toErase.Clear();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void OnComponentAdded(IEntity entity, Type componentType)
        {
            ComponentAdded?.Invoke(entity, componentType);
        }

        internal void OnComponentRemoved(IEntity entity, Type componentType)
        {
            ComponentRemoved?.Invoke(entity, componentType);
        }

        internal void Raise<T>(IEntity entity, T eventArgs) where T : EventArgs
        {
            eventsMan.Raise<T>(entity, eventArgs);
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddToLookup(string tag, IEntity entity)
        {
            if (tag is null)
            {
                entityByNoTagLookup.Add(entity);
                return;
            }

            if (!entityByTagLookup.TryGetValue(tag, out HashSet<IEntity> foundEntities))
            {
                foundEntities = new HashSet<IEntity>();
                entityByTagLookup.Add(tag, foundEntities);
            }

            foundEntities.Add(entity);
        }

        private void RemoveFromLookup(string tag, IEntity entity)
        {
            if (tag is null)
            {
                entityByNoTagLookup.Remove(entity);
                return;
            }

            if (!entityByTagLookup.TryGetValue(tag, out HashSet<IEntity> foundEntities))
                return;

            foundEntities.Remove(entity);

            if (foundEntities.Any())
                return;

            entityByTagLookup.Remove(tag);
        }

        #endregion Private Methods
    }
}