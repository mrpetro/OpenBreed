using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreEventBus
    {
        #region Private Fields

        private readonly Queue<EventSubscription> queue = new Queue<EventSubscription>();
        private Dictionary<object, Dictionary<string, List<Action<object, IEvent>>>> listeners = new Dictionary<object, Dictionary<string, List<Action<object, IEvent>>>>();

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

        public void Subscribe(object sender, string eventType, Action<object, IEvent> callback)
        {
            Dictionary<string,List<Action<object, IEvent>>> eventTypes = null;
            List<Action<object,IEvent>> callbacks = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
            {
                eventTypes = new Dictionary<string, List<Action<object, IEvent>>>();
                listeners.Add(sender, eventTypes);
            }

            if (!eventTypes.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<Action<object, IEvent>>();
                eventTypes.Add(eventType, callbacks);
            }

            callbacks.Add(callback);
        }

        public void Unsubscribe(object sender, string eventType, Action<object, IEvent> callback)
        {
            Dictionary<string, List<Action<object, IEvent>>> eventTypes = null;
  
            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<Action<object, IEvent>> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove(callback);

            //if (callbacks.Count == 0)
            //    eventTypes.Remove(eventType);

            //if (eventTypes.Count == 0)
            //    listeners.Remove(sender);
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(object sender, IEvent ev)
        {
            Dictionary<string, List<Action<object, IEvent>>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<Action<object, IEvent>> callbacks = null;

            if (!eventTypes.TryGetValue(ev.Type, out callbacks))
                return;

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