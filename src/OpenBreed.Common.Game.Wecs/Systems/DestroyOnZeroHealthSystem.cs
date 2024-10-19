using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(HealthComponent))]
    internal class DestroyOnZeroHealthSystem : UpdatableMatchingSystemBase<DestroyOnZeroHealthSystem>
    {
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        internal DestroyOnZeroHealthSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var healthComponent = entity.Get<HealthComponent>();

            if (healthComponent.Value > 0)
            {
                return;
            }
            
            eventsMan.Raise(new DestroyedEvent(entity.Id));
        }
    }
}
