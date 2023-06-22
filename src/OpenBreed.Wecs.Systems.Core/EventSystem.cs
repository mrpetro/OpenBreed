using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class EventSystem<TEvent, TSystem> : SystemBase<TSystem>, IEventSystem<TEvent> where TEvent : EventArgs where TSystem : ISystem
    {
        #region Private Fields

        protected readonly HashSet<IEntity> entities = new HashSet<IEntity>();

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

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        public abstract void Update(object sender, TEvent e);

        #endregion Public Methods
    }
}