using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    public class EventsMan
    {
        #region Private Fields

        private Dictionary<object, Dictionary<string, List<Action<object, EventArgs>>>> listeners = new Dictionary<object, Dictionary<string, List<Action<object, EventArgs>>>>();

        #endregion Private Fields

        #region Public Constructors

        public EventsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Raise(object sender, string eventName, EventArgs eventArgs)
        {
            NotifyListeners(sender, eventName, eventArgs);
        }

        public void Subscribe(object sender, string eventType, Action<object, EventArgs> callback)
        {
            Dictionary<string, List<Action<object, EventArgs>>> eventTypes = null;
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
    }
}