using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreMessageBus
    {
        #region Private Fields

        private readonly Queue<MsgData> queue = new Queue<MsgData>();

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

        public void Enqueue(object sender, IMsg msg)
        {
            queue.Enqueue(new MsgData(sender, msg));
        }

        public void PostEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                Post(ed.Sender, ed.Msg);
            }
        }

        public void RegisterHandler(string msgType, IMsgHandler msgHandler)
        {
            handlers.Add(msgType, msgHandler);
        }

        #endregion Public Methods

        #region Private Methods

        private void Post(object sender, IMsg msg)
        {
            if (msg is IEntityMsg)
            {
                Post(sender, (IEntityMsg)msg);
                return;
            }

            IMsgHandler handler = null;
            if (handlers.TryGetValue(msg.Type, out handler))
                handler.HandleMsg(sender, msg);
        }

        private void Post(object sender, IEntityMsg msg)
        {
            Debug.Assert(msg != null);
            Debug.Assert(msg.Entity != null);
            Debug.Assert(msg.Entity.World != null);

            msg.Entity.World.MessageBus.HandleMsg(sender, msg);
        }

        #endregion Private Methods

        #region Private Structs

        private struct MsgData
        {
            #region Internal Fields

            internal object Sender;
            internal IMsg Msg;

            #endregion Internal Fields

            #region Internal Constructors

            internal MsgData(object sender, IMsg msg)
            {
                Sender = sender;
                Msg = msg;
            }

            #endregion Internal Constructors
        }

        #endregion Private Structs
    }
}