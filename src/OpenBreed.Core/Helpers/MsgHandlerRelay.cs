﻿using OpenBreed.Core.Commands;
using System.Collections.Generic;

namespace OpenBreed.Core.Helpers
{
    public class MsgHandlerRelay : IMsgHandler
    {
        #region Private Fields

        private Dictionary<string, IMsgHandler> relayHandlers = new Dictionary<string, IMsgHandler>();

        #endregion Private Fields

        #region Public Constructors

        public MsgHandlerRelay(IMsgHandler handler)
        {
            Handler = handler;
        }

        #endregion Public Constructors

        #region Public Properties

        public IMsgHandler Handler { get; }

        #endregion Public Properties

        #region Public Methods

        internal void RegisterHandler(string msgType, IMsgHandler msgHandler)
        {
            relayHandlers.Add(msgType, msgHandler);
        }

        public bool Handle(object sender, IMsg msg)
        {
            IMsgHandler handler = null;
            if (relayHandlers.TryGetValue(msg.Type, out handler))
                handler.Handle(sender, msg);

            return true;
        }

        #endregion Public Methods
    }
}