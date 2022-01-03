using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Entities
{
    public class EntityJob : IJob
    {
        #region Private Fields

        private readonly Action action;

        #endregion Private Fields

        #region Public Constructors

        public EntityJob(Action action)
        {
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
            action.Invoke();
            Complete(this);
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods
    }

    public class EntityJob<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private Entity entity;
        private Action action;

        #endregion Private Fields

        #region Public Constructors

        public EntityJob(Entity entity, Action action)
        {
            this.entity = entity;
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
            entity.Subscribe<TEventArgs>(OnTrigger);
            action.Invoke();
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnTrigger(object sender, TEventArgs args)
        {
            entity.Unsubscribe<TEventArgs>(OnTrigger);
            Complete(this);
        }

        #endregion Private Methods
    }
}