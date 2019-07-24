using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreMessageBus
    {
        #region Private Fields

        private Dictionary<string, IMsgHandler> handlers = new Dictionary<string, IMsgHandler>();

        #endregion Private Fields

        #region Public Constructors

        public CoreMessageBus(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void RegisterHandler(string msgType, IMsgHandler msgHandler)
        {
            handlers.Add(msgType, msgHandler);
        }

        public void PostMsg(object sender, IMsg msg)
        {
            if (msg is IEntityMsg)
            {
                PostEntityMsg(sender, (IEntityMsg)msg);
                return;
            }

            IMsgHandler handler = null;
            if (handlers.TryGetValue(msg.Type, out handler))
                handler.HandleMsg(sender, msg);
        }

        private void PostEntityMsg(object sender, IEntityMsg msg)
        {
            Debug.Assert(msg != null);
            Debug.Assert(msg.Entity != null);
            Debug.Assert(msg.Entity.World != null);

            msg.Entity.World.MessageBus.HandleMsg(sender, msg);
        }

        #endregion Public Methods
    }
}