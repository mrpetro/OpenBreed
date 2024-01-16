using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    public class PositionObserveSystem : EventSystem<PositionChangedEvent, PositionObserveSystem>
    {
        public PositionObserveSystem(IEventsMan eventsMan) : base(eventsMan)
        {
        }

        public override void Update(object sender, PositionChangedEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
