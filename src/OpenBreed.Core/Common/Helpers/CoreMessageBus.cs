using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreMessageBus
    {
        #region Private Fields

        private readonly Queue<MsgData> queue = new Queue<MsgData>();

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

        #endregion Public Methods

        #region Private Methods

        private void Post(object sender, IMsg msg)
        {
            if (msg is IEntityMsg)
            {
                Post(sender, (IEntityMsg)msg);
                return;
            }
            else if (msg is IWorldMsg)
            {
                Post(sender, (IWorldMsg)msg);
                return;
            }
        }

        private void Post(object sender, IEntityMsg msg)
        {
            Debug.Assert(msg != null);
            Debug.Assert(msg.Entity != null);
            Debug.Assert(msg.Entity.World != null);

            msg.Entity.World.MessageBus.PostMsg(sender, msg);
        }

        private void Post(object sender, IWorldMsg msg)
        {
            Debug.Assert(msg != null);

            Core.Worlds.PostMsg(sender, msg);
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