using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Events
{
    public class ComponentChangedEventHandler : IEventHandler<IComponentChangedEvent>
    {
        private Queue<IComponentChangedEvent> enqueued = new Queue<IComponentChangedEvent>();

        public void Enqueue(IEvent e)
        {
            enqueued.Enqueue((IComponentChangedEvent)e);
        }

        public void Fire()
        {
            while (enqueued.Count > 0)
                Handle(enqueued.Dequeue());
        }

        public void Handle(IComponentChangedEvent eventArgs)
        {
            //throw new NotImplementedException();
        }
    }
}
