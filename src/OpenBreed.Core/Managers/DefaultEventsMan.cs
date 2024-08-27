using OpenBreed.Core.Interface.Managers;
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

        public void Raise<TEventArgs>(object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            NotifyListeners(eventArgs.GetType(), eventArgs);
        }

        public void Raise<TEventArgs>(TEventArgs eventArgs) where TEventArgs : EventArgs => Raise<TEventArgs>(null, eventArgs);

        public void Subscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);

            if (!listeners.TryGetValue(eventType, out List<(object, MethodInfo)> callbacks))
            {
                callbacks = new List<(object, MethodInfo)>();
                listeners.Add(eventType, callbacks);
            }

            callbacks.Add((callback.Target, callback.Method));
        }

        public void Unsubscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);

            if (!listeners.TryGetValue(eventType, out List<(object, MethodInfo)> callbacks))
            {
                return;
            }

            callbacks.Remove((callback.Target, callback.Method));
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyListeners(Type eventType, EventArgs eventArgs)
        {
            List<(object Target, MethodInfo Method)> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
                return;

            for (int i = 0; i < callbacks.Count; i++)
            {
                var item = callbacks[i];
                item.Method.Invoke(item.Target, new object[] { eventArgs });
            }
        }

        #endregion Private Methods
    }
}