using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    public class EventsMan
    {
        #region Private Fields

        private Dictionary<object, Dictionary<Type, List<(object, MethodInfo)>>> listeners = new Dictionary<object, Dictionary<Type, List<(object, MethodInfo)>>>();

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
            NotifyListeners(sender, eventArgs.GetType(), eventArgs);
        }

        public void Subscribe<T>(object sender, Action<object, T> callback) where T : EventArgs
        {
            var eventType = typeof(T);
            Dictionary<Type, List<(object, MethodInfo)>> eventTypes = null;
            List<(object, MethodInfo)> callbacks = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
            {
                eventTypes = new Dictionary<Type, List<(object, MethodInfo)>>();
                listeners.Add(sender, eventTypes);
            }

            if (!eventTypes.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<(object, MethodInfo)>();
                eventTypes.Add(eventType, callbacks);
            }

            callbacks.Add((callback.Target, callback.Method));
        }

        public void Unsubscribe<T>(object sender, Action<object, T> callback) where T : EventArgs
        {
            var eventType = typeof(T);
            Dictionary<Type, List<(object, MethodInfo)>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<(object, MethodInfo)> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove((callback.Target, callback.Method));
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(object sender, Type eventType, EventArgs eventArgs)
        {
            Dictionary<Type, List<(object, MethodInfo)>> eventTypes = null;

            if (!listeners.TryGetValue(sender, out eventTypes))
                return;

            List<(object Target, MethodInfo Method)> callbacks = null;

            if (!eventTypes.TryGetValue(eventType, out callbacks))
                return;

            for (int i = 0; i < callbacks.Count; i++)
            {
                var item = callbacks[i];
                item.Method.Invoke(item.Target, new object[] { sender, eventArgs });
            }

            //callbacks.ForEach(item => item.Method.Invoke(item.Target, new object[] { sender, eventArgs }));
        }

        #endregion Private Methods
    }
}