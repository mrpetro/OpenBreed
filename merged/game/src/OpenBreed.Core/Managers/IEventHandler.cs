using OpenBreed.Core.Events;
using System;

namespace OpenBreed.Core.Managers
{
    public interface IEventHandler
    {
        void Fire();
        void Enqueue(IEvent e);
    }

    public interface IEventHandler<TEvent> : IEventHandler where TEvent : IEvent
    {
    }
}