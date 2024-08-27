using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    public abstract class EntityEventSystem<TSystem, TEntityEvent> : IEventSystem<TEntityEvent> where TEntityEvent : EntityEvent
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

        public abstract void Update(TEntityEvent e);

        #endregion Public Methods
    }
}