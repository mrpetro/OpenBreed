using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core
{
    public class EntityJobEx2<TEventArgs> : IJob where TEventArgs : EventArgs
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

        private void OnTrigger(object sender, TEventArgs args)
        {
            entity.Unsubscribe<TEventArgs>(OnTrigger);
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
            entity.Subscribe<TEventArgs>(OnTrigger);
            action.Invoke();
        }

        public void Update(float dt)
        {

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