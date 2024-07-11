using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    internal class DefaultEventsMan : IEventsMan
    {
        #region Private Fields

        private readonly Dictionary<Type, List<(object, MethodInfo)>> listeners = new Dictionary<Type, List<(object, MethodInfo)>>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultEventsMan()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Raise<T>(object sender, T eventArgs) where T : EventArgs
        {
            NotifyListeners(sender, eventArgs.GetType(), eventArgs);
        }

        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            var eventType = typeof(T);
            List<(object, MethodInfo)> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
            {
                callbacks = new List<(object, MethodInfo)>();
                listeners.Add(eventType, callbacks);
            }

            callbacks.Add((callback.Target, callback.Method));
        }

        public void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            var eventType = typeof(T);

            List<(object, MethodInfo)> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
                return;

            callbacks.Remove((callback.Target, callback.Method));
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(object sender, Type eventType, EventArgs eventArgs)
        {
            List<(object Target, MethodInfo Method)> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
                return;

            for (int i = 0; i < callbacks.Count; i++)
            {
                var item = callbacks[i];
                item.Method.Invoke(item.Target, new object[] { sender, eventArgs });
            }
        }

        #endregion Private Methods
    }
}