using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    public class EventsMan
    {
        #region Private Fields

        private Dictionary<object, Dictionary<string, List<(object, MethodInfo)>>> listeners = new Dictionary<object, Dictionary<string, List<(object, MethodInfo)>>>();

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

        public void Raise<T>(object sender, T eventArgs) where T : EventArgs
        {
            Raise(sender, eventArgs.GetType().FullName, eventArgs);
        }

        public void Subscribe<T>(object sender, Action<object, T> callback) where T : EventArgs
        {
            Subscribe(sender, typeof(T).FullName, (callback.Target, callback.Method));
        }

        public void Unsubscribe<T>(object sender, Action<object, T> callback) where T : EventArgs
        {
            Unsubscribe(sender, typeof(T).FullName, (callback.Target, callback.Method));
        }

        #endregion Public Methods

        #region Private Methods

        private void Raise(object sender, string eventName, EventArgs eventArgs)
        {
            NotifyListeners(sender, eventName, eventArgs);
        }

        private void Unsubscribe(object sender, string eventType, (object, MethodInfo) callback)
        {
            Dictionary<string, List<(object, MethodInfo)>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<(object, MethodInfo)> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove(callback);
        }

        private void Subscribe(object sender, string eventType, (object, MethodInfo) callback)
        {
            Dictionary<string, List<(object, MethodInfo)>> eventTypes = null;
            List<(object, MethodInfo)> callbacks = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
            {
                eventTypes = new Dictionary<string, List<(object, MethodInfo)>>();
                listeners.Add(sender, eventTypes);
            }

            if (!eventTypes.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<(object, MethodInfo)>();
                eventTypes.Add(eventType, callbacks);
            }

            callbacks.Add(callback);
        }

        private void NotifyListeners(object sender, string eventType, EventArgs eventArgs)
        {
            Dictionary<string, List<(object, MethodInfo)>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<(object Target, MethodInfo Method)> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.ForEach(item => item.Method.Invoke(item.Target, new object[] { sender, eventArgs }));
        }

        #endregion Private Methods
    }
}