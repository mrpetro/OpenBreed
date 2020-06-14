﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Helpers;
using System;
using System.Diagnostics;

namespace OpenBreed.Core.Managers
{
    public class CommandsMan
    {
        #region Public Constructors

        public CommandsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors
        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Post(object sender, IMsg msg)
        {
            Debug.Assert(msg != null);

            if (Core.CanHandle(msg.Type))
            {
                Core.HandleCmd(sender, msg);
                return;
            }

            if (msg is IEntityCommand)
            {
                Post(sender, (IEntityCommand)msg);
                return;
            }
            else if (msg is IWorldCommand)
            {
                Post(sender, (IWorldCommand)msg);
                return;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Post(object sender, IEntityCommand msg)
        {
            Core.Entities.HandleCmd(sender, msg);
        }

        private void Post(object sender, IWorldCommand msg)
        {
            Core.Worlds.HandleCmd(sender, msg);
        }

        #endregion Private Methods
    }
}