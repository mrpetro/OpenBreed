using OpenBreed.Common.Logging;
using OpenBreed.Core.Events;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Managers
{
    public class EventQueue : IEventQueue
    {
        #region Private Fields

        private readonly Dictionary<Type, IEventHandler> eventHandlers = new Dictionary<Type, IEventHandler>();

        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public EventQueue(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Enqueue<TEvent>(TEvent e) where TEvent : IEvent => Enqueue(e);

        public void Enqueue(IEvent e)
        {
            if (eventHandlers.TryGetValue(e.GetType(), out IEventHandler eventHandler))
                eventHandler.Enqueue(e);
            else
                logger.Warning($"Event of type '{e.GetType()}' had no handler available.");
        }

        public void AddHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);

            if (eventHandlers.ContainsKey(eventType))
                throw new InvalidOperationException($"Handler for event type '{eventType}' already added.");

            eventHandlers.Add(eventType, handler);
        }

        public void Fire()
        {
            foreach (var handler in eventHandlers.Values)
                handler.Fire();
        }

        #endregion Public Methods
    }
}