using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Events;
using OpenBreed.Core.Interface.Managers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    public class MessagesMan : IMessagesMan
    {
        #region Private Fields

        private readonly IdMap<IMsg> messages = new IdMap<IMsg>();

        #endregion Private Fields

        #region Public Methods

        public void RemoveMsgData(int msgId)
        {
            messages.RemoveById(msgId);
        }

        public IMsg GetMsgData(int msgId)
        {
            return messages[msgId];
        }

        public int SetMsgData(IMsg data)
        {
            return messages.Add(data);
        }

        #endregion Public Methods
    }
}