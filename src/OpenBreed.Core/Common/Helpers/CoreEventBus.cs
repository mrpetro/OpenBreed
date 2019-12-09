using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Common.Helpers
{
    public class CoreEventBus
    {
        #region Private Fields

        private readonly Queue<EventSubscription> queue = new Queue<EventSubscription>();
        private Dictionary<object, Dictionary<string, List<Action<object, EventArgs>>>> listeners = new Dictionary<object, Dictionary<string, List<Action<object, EventArgs>>>>();

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

        public void Enqueue(object sender, string eventName, EventArgs eventArgs)
        {
            queue.Enqueue(new EventSubscription(sender, eventName, eventArgs));
        }

        public void RaiseEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                NotifyListeners(ed.Sender, ed.EventType, ed.EventArgs);
            }
        }

        public void Subscribe(object sender, string eventType, Action<object, EventArgs> callback)
        {
            Dictionary<string,List<Action<object, EventArgs>>> eventTypes = null;
            List<Action<object, EventArgs>> callbacks = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
            {
                eventTypes = new Dictionary<string, List<Action<object, EventArgs>>>();
                listeners.Add(sender, eventTypes);
            }

            if (!eventTypes.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<Action<object, EventArgs>>();
                eventTypes.Add(eventType, callbacks);
            }

            callbacks.Add(callback);
        }

        public void Unsubscribe(object sender, string eventType, Action<object, EventArgs> callback)
        {
            Dictionary<string, List<Action<object, EventArgs>>> eventTypes = null;
  
            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<Action<object, EventArgs>> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove(callback);
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(object sender, string eventType, EventArgs eventArgs)
        {
            Dictionary<string, List<Action<object, EventArgs>>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<Action<object, EventArgs>> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.ForEach(item => item(sender, eventArgs));
        }

        #endregion Private Methods

        #region Private Structs

        private struct EventSubscription
        {
            #region Internal Fields

            internal object Sender;
            internal string EventType;
            internal EventArgs EventArgs;

            #endregion Internal Fields

            #region Internal Constructors

            internal EventSubscription(object sender, string eventType, EventArgs eventArgs)
            {
                Sender = sender;
                EventType = eventType;
                EventArgs = eventArgs;
            }

            #endregion Internal Constructors
        }

        #endregion Private Structs
    }
}