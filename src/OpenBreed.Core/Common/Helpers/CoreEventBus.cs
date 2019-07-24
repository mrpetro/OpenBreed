using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreEventBus
    {
        #region Private Fields

        private readonly Queue<EventData> queue = new Queue<EventData>();

        #endregion Private Fields

        #region Public Constructors

        public CoreEventBus(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Enqueue(object sender, IEvent e)
        {
            queue.Enqueue(new EventData(sender, e));
        }

        public void RaiseEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                Raise(ed.Sender, ed.Event);
            }
        }

        public void Raise(object sender, IEvent ev)
        {
            //if (ev is IEvent)
            //{
            //    PostEntityMsg(sender, (IEntityMsg)ev);
            //    return;
            //}

            //IMsgHandler handler = null;
            //if (handlers.TryGetValue(ev.Type, out handler))
            //    handler.HandleMsg(sender, ev);
        }

        #endregion Public Methods

        #region Private Structs

        private struct EventData
        {
            #region Internal Fields

            internal object Sender;
            internal IEvent Event;

            #endregion Internal Fields

            #region Internal Constructors

            internal EventData(object sender, IEvent e)
            {
                Sender = sender;
                Event = e;
            }

            #endregion Internal Constructors
        }

        #endregion Private Structs
    }
}