using OpenBreed.Core.Events;

namespace OpenBreed.Core.Managers
{
    public interface IEventQueue
    {
        #region Public Methods

        void Enqueue<TEvent>(TEvent e) where TEvent : IEvent;

        void Enqueue(IEvent e);

        void AddHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        void Fire();

        #endregion Public Methods
    }
}