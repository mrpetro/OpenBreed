using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class EventMatchingSystem<TEvent, TSystem> : MatchingSystemBase<TSystem>, IEventSystem<TEvent> where TEvent : EventArgs where TSystem : IMatchingSystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected EventMatchingSystem(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan;

            eventsMan.Subscribe<TEvent>(Update);
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Update(TEvent e);

        #endregion Public Methods
    }
}