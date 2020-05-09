using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core
{
    public class WorldJobEx<TEventArgs> : IJob where TEventArgs : EventArgs
    {
        #region Private Fields

        private World world;
        private ICommand command;

        #endregion Private Fields

        #region Private Constructors



        public WorldJobEx(World world, ICommand command)
        {
            this.world = world;
            this.command = command;
        }

        private void OnTrigger(object sender, TEventArgs args)
        {
            world.Unsubscribe<TEventArgs>(OnTrigger);
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
            world.Subscribe<TEventArgs>(OnTrigger);
            world.PostCommand(command);
        }

        public void Update(float dt)
        {

        }

        #endregion Public Methods
    }
}