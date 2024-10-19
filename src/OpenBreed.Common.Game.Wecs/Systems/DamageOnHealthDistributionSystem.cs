using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(DamagerComponent))]
    internal class DamageOnHealthDistributionSystem : UpdatableMatchingSystemBase<DamageOnHealthDistributionSystem>
    {
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        internal DamageOnHealthDistributionSystem(
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var damageComponent = entity.Get<DamagerComponent>();

            var toDistribute = damageComponent.Inflictions;

            for (int i = 0; i < toDistribute.Count; i++)
            {
                Inflict(entity, toDistribute[i]);
            }

            toDistribute.Clear();
        }

        private void Inflict(IEntity damagingEntity, DamageInfliction damageDistribution)
        {
            for (int i = 0; i < damageDistribution.Targets.Length; i++)
            {
                InflictToEntity(damagingEntity, damageDistribution.Amount, damageDistribution.Targets[i]);
            }
        }

        private void InflictToEntity(IEntity damagingEntity, int damage, int targetEntityId)
        {
            var targetEntity = entityMan.GetById(targetEntityId);

            if(targetEntity is null)
            {
                logger.LogError("Target entity with ID '{0}' not found.", targetEntityId);
                return;
            }

            var healthComponent = targetEntity.TryGet<HealthComponent>();

            if(healthComponent is null)
            {
                logger.LogError("Target entity with ID '{0}' has no HealthComponent.", targetEntityId);
                return;
            }

            healthComponent.Value -= damage;
            eventsMan.Raise(new DamagedEvent(targetEntityId, damage, damagingEntity.Id));
        }
    }
}
