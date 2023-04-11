﻿using OpenBreed.Audio.OpenAL.Managers;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Audio.Events;
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
        typeof(DamagerComponent))]
    internal class DamageOnHealthDistributionSystem : UpdatableSystemBase<DamageOnHealthDistributionSystem>
    {
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        internal DamageOnHealthDistributionSystem(
            IWorld world,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
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
                logger.Error($"Target entity with ID '{targetEntityId}' not found.");
                return;
            }

            var healthComponent = targetEntity.TryGet<HealthComponent>();

            if(healthComponent is null)
            {
                logger.Error($"Target entity with ID '{targetEntityId}' has no HealthComponent.");
                return;
            }

            healthComponent.Value -= damage;
            eventsMan.Raise(targetEntity, new DamagedEvent(targetEntityId, damage, damagingEntity.Id));
        }
    }
}
