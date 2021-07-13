using OpenBreed.Core;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Wecs.Worlds
{
    public class WorldJob<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private Action action;

        private Func<object, TEventArgs, bool> triggerFunc;
        private IWorldMan worldMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public WorldJob(IWorldMan worldMan, IEventsMan eventsMan, Func<object, TEventArgs, bool> triggerFunc, Action action)
        {
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;
            this.triggerFunc = triggerFunc;
            this.action = action;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public void Execute()
        {
            eventsMan.Subscribe<TEventArgs>(worldMan, OnEvent);
            action.Invoke();
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEvent(object sender, TEventArgs args)
        {
            if (!triggerFunc.Invoke(sender, args))
                return;

            eventsMan.Unsubscribe<TEventArgs>(worldMan, OnEvent);
            Complete(this);
        }

        #endregion Private Methods
    }
}