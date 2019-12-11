using OpenBreed.Core.Commands;
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

        private void Post(object sender, IEntityCommand msg)
        {
            Debug.Assert(msg != null);
            Debug.Assert(msg.EntityId >= 0);

            var entity = Core.Entities.GetById(msg.EntityId);
            entity.World.MessageBus.PostCommand(sender, msg);
        }

        private void Post(object sender, IWorldCommand msg)
        {
            Debug.Assert(msg != null);

            Core.Worlds.PostCommand(sender, msg);
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