﻿using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    public abstract class EntityEventSystem<TSystem, TEntityEvent> : MatchingSystemBase<TSystem>, IEventSystem<TEntityEvent> where TSystem : IMatchingSystem where TEntityEvent : EntityEvent
    {
        #region Protected Fields

        protected readonly IEventsMan eventsMan;

        #endregion Protected Fields

        #region Protected Constructors

        protected EntityEventSystem(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan;

            eventsMan.Subscribe<TEntityEvent>(Update);
        }

        #endregion Protected Constructors

        #region Public Methods

        public void Update(object sender, TEntityEvent e)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity, e);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(IEntity entity, TEntityEvent e);

        #endregion Protected Methods
    }
}