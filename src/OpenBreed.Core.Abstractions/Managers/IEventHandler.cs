using OpenBreed.Core.Abstractions.Events;
using System;

namespace OpenBreed.Core.Abstractions.Managers
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