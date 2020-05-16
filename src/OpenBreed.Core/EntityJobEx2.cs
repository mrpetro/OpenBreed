using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core
{
    public class WorldJobEx2<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private Action action;

        #endregion Private Fields

        #region Private Constructors

        private ICore core;
        private string worldName;

        public WorldJobEx2(ICore core, string worldName , Action action)
        {
            this.core = core;
            this.worldName = worldName;
            this.action = action;
        }

        internal World World => core.Worlds.GetByName(worldName);

        private void OnTrigger(object sender, TEventArgs args)
        {
            World.Unsubscribe<TEventArgs>(OnTrigger);
            Complete(this);
        }

        #endregion Private Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public void Execute()
        {
            World.Subscribe<TEventArgs>(OnTrigger);
            action.Invoke();
        }

        public void Update(float dt)
        {

        }

        public void CompleteTrigger<T>()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }


    public class EntityJobEx2 : IJob
    {
        #region Private Fields

        private IEntity entity;
        private Action action;

        #endregion Private Fields

        #region Private Constructors

        public EntityJobEx2(IEntity entity, Action action)
        {
            this.entity = entity;
            this.action = action;
        }

        #endregion Private Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
        }

        public void Execute()
        {
            action.Invoke();
            Complete(this);
        }

        public void Update(float dt)
        {

        }

        #endregion Public Methods
    }
}