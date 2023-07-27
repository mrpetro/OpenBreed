using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(ResurrectableComponent),
        typeof(LivesComponent))]
    public class ResurrectionSystem : EntityEventSystem<ResurrectionSystem, EntityEnteredEvent>
    {
        public ResurrectionSystem(IEventsMan eventsMan) : base(eventsMan)
        {
        }

        protected override void UpdateEntity(IEntity entity, EntityEnteredEvent e)
        {
            var livesComponent = entity.Get<LivesComponent>();

            if (livesComponent.Value > 0)
                return;


        }
    }
}
