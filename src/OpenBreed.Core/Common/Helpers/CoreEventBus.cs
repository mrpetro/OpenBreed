using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreEventBus
    {
        #region Private Fields

        private readonly Queue<EventSubscription> queue = new Queue<EventSubscription>();
        private Dictionary<string, List<Action<object, IEvent>>> listeners = new Dictionary<string, List<Action<object, IEvent>>>();

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
            queue.Enqueue(new EventSubscription(sender, e));
        }

        public void RaiseEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                NotifyListeners(ed.Sender, ed.Event);
            }
        }

        public void Subscribe(string eventType, Action<object, IEvent> callback)
        {
            List<Action<object, IEvent>> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<Action<object, IEvent>>();
                listeners.Add(eventType, callbacks);
            }

            callbacks.Add(callback);
        }

        public void Unsubscribe(string eventType, Action<object, IEvent> callback)
        {
            List<Action<object, IEvent>> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove(callback);
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(object sender, IEvent ev)
        {
            List<Action<object, IEvent>> callbacks = null;

            if (listeners.TryGetValue(ev.Type, out callbacks))
                callbacks.ForEach(item => item(sender, ev));
        }

        #endregion Private Methods

        #region Private Structs

        private struct EventSubscription
        {
            #region Internal Fields

            internal object Sender;
            internal IEvent Event;

            #endregion Internal Fields

            #region Internal Constructors

            internal EventSubscription(object sender, IEvent e)
            {
                Sender = sender;
                Event = e;
            }

            #endregion Internal Constructors
        }

        #endregion Private Structs
    }
}