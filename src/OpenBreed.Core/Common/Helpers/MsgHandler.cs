using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class MsgHandler : IMsgHandler
    {
        #region Private Fields

        private readonly Queue<Tuple<object, IMsg>> queue = new Queue<Tuple<object, IMsg>>();

        private readonly IMsgListener listener;

        #endregion Private Fields

        #region Public Constructors

        public MsgHandler(IMsgListener listener)
        {
            this.listener = listener;
        }

        #endregion Public Constructors

        #region Public Methods

        public void PostEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                listener.RecieveMsg(ed.Item1, ed.Item2);
            }
        }

        public bool EnqueueMsg(object sender, IMsg msg)
        {
            queue.Enqueue(new Tuple<object, IMsg>(sender, msg));
            return true;
        }

        public bool RecieveMsg(object sender, IMsg msg)
        {
            return listener.RecieveMsg(sender, msg);
        }

        #endregion Public Methods
    }
}