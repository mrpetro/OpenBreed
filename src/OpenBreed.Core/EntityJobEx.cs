using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core
{
    public class EntityJobEx<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private IEntity entity;
        private ICommand command;

        #endregion Private Fields

        #region Private Constructors



        public EntityJobEx(IEntity entity, ICommand command)
        {
            this.entity = entity;
            this.command = command;
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
            entity.World.Subscribe<TEventArgs>(OnTrigger);
            entity.PostCommand(command);
        }

        public void Update(float dt)
        {

        }

        #endregion Public Methods
    }
}