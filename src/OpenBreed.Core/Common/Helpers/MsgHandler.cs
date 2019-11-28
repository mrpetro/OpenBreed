using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class MsgHandler : IMsgHandler
    {
        #region Private Fields

        private readonly IMsgListener listener;

        #endregion Private Fields

        #region Public Constructors

        public MsgHandler(IMsgListener listener)
        {
            this.listener = listener;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool HandleMsg(object sender, IMsg msg)
        {
            return listener.RecieveMsg(sender, msg);
        }

        #endregion Public Methods
    }
}