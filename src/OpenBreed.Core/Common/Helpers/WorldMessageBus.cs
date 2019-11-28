using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Common.Helpers
{
    public class WorldMessageBus
    {
        #region Private Fields

        private Dictionary<string, IMsgHandler> handlers = new Dictionary<string, IMsgHandler>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMessageBus(World world)
        {
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public World World { get; }

        #endregion Public Properties

        #region Public Methods

        public void RegisterHandler(string msgType, IMsgHandler msgHandler)
        {
            handlers.Add(msgType, msgHandler);
        }

        public void PostMsg(object sender, IMsg msg)
        {
            IMsgHandler handler = null;
            if (handlers.TryGetValue(msg.Type, out handler))
            {
                handler.HandleMsg(sender, msg);
            }
        }


        #endregion Public Methods
    }
}