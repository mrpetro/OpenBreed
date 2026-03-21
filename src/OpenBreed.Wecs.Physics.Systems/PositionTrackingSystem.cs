using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems
{
    [RequireEntityWith(
        typeof(PositionComponent),
        typeof(PreviousPositionComponent))]
    public class PositionTrackingSystem : IUpdatableSystem
    {
        private readonly IEventsMan eventsMan;

        public PositionTrackingSystem(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
        }

        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                var previous = entity.Get<PreviousPositionComponent>();
                var current = entity.Get<PositionComponent>();

                var delta = current.Value - previous.Value;

                if (delta.X == 0 && delta.Y == 0)
                {
                    continue;
                }

                eventsMan.Raise(new PositionChangedEvent(entity.Id, current.Value));
                previous.Value = current.Value;
            }
        }
    }
}
