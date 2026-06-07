using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    internal class DefaultEventsMan : IEventsMan
    {
        #region Private Fields

        private readonly ConcurrentQueue<Action> mainThreadActions = new();
        private readonly Dictionary<Type, List<Delegate>> listeners = new Dictionary<Type, List<Delegate>>();

        #endregion Private Fields

        #region Public Constructors

        public DefaultEventsMan()
        {
            Subscribe<WindowUpdateEvent>((e) => NotifyMainThread());
        }

        #endregion Public Constructors

        #region Public Methods

        public void Raise<TEventArgs>(object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            NotifyListeners(eventArgs.GetType(), eventArgs);
        }

        public void Raise<TEventArgs>(TEventArgs eventArgs) where TEventArgs : EventArgs => Raise<TEventArgs>(null, eventArgs);

        public void Subscribe(Type eventType, Delegate callback)
        {
            if (!listeners.TryGetValue(eventType, out List<Delegate> callbacks))
            {
                callbacks = new List<Delegate>();
                listeners.Add(eventType, callbacks);
            }

            callbacks.Add(callback);
        }



        public void Subscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs
            => Subscribe(typeof(TEventArgs), callback);

        public void Unsubscribe<TEventArgs>(EventCallback<TEventArgs> callback) where TEventArgs : EventArgs
        {
            var eventType = typeof(TEventArgs);

            if (!listeners.TryGetValue(eventType, out List<Delegate> callbacks))
            {
                return;
            }

            callbacks.Remove(callback);
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyMainThread()
        {
            while (mainThreadActions.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }

        private void NotifyListeners(Type eventType, EventArgs eventArgs)
        {
            List<Delegate> callbacks = null;

            if (!listeners.TryGetValue(eventType, out callbacks))
            {
                return;
            }

            if (!ThreadTools.IsMainThread)
            {
                for (int i = 0; i < callbacks.Count; i++)
                {
                    var callback = callbacks[i];
                    mainThreadActions.Enqueue(() => callback.DynamicInvoke(eventArgs));
                }

                return;
            }

            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i].DynamicInvoke(eventArgs);
            }
        }

        #endregion Private Methods
    }
}