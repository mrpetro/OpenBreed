using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Core
{
    public abstract class EventSystem<TEvent, TSystem> : SystemBase<TSystem>, IEventSystem<TEvent> where TEvent : EventArgs where TSystem : ISystem
    {
        #region Protected Constructors

        protected EventSystem()
        {

        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Update(TEvent e);

        #endregion Public Methods
    }
}