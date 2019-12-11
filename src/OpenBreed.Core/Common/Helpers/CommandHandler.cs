using OpenBreed.Core.Commands;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class CommandHandler : IMsgHandler
    {
        #region Private Fields

        private readonly ICommandListener listener;

        #endregion Private Fields

        #region Public Constructors

        public CommandHandler(ICommandListener listener)
        {
            this.listener = listener;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool Handle(object sender, IMsg cmd)
        {
            return listener.RecieveCommand(sender, (ICommand)cmd);
        }

        #endregion Public Methods
    }
}