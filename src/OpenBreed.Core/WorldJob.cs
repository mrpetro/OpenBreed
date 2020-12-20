using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Core
{
    public class WorldJob<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private Action action;

        private Func<object, TEventArgs, bool> triggerFunc;
        private IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public WorldJob(IWorldMan worldMan, Func<object, TEventArgs, bool> triggerFunc, Action action)
        {
            this.worldMan = worldMan;
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
            worldMan.Subscribe<TEventArgs>(OnEvent);
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

            worldMan.Unsubscribe<TEventArgs>(OnEvent);
            Complete(this);
        }

        #endregion Private Methods
    }
}