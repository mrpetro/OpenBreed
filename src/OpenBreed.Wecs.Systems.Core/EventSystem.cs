using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class EventSystem<TEvent, TSystem> : SystemBase<TSystem>, IEventSystem<TEvent> where TEvent : EventArgs where TSystem : ISystem
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected EventSystem(IEventsMan eventsMan)
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