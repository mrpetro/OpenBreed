using OpenBreed.Core.Interface.Events;
using System;

namespace OpenBreed.Core.Interface.Managers
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