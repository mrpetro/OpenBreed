using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Core.Systems;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(DamagerComponent))]
    public class DamageOnHealthDistributionSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public DamageOnHealthDistributionSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger) : base(worldMan)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Methods

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

        #endregion Protected Methods

        #region Private Methods

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

            if (targetEntity is null)
            {
                logger.LogError("Target entity with ID '{0}' not found.", targetEntityId);
                return;
            }

            var healthComponent = targetEntity.TryGet<HealthComponent>();

            if (healthComponent is null)
            {
                logger.LogError("Target entity with ID '{0}' has no HealthComponent.", targetEntityId);
                return;
            }

            healthComponent.Value -= damage;
            eventsMan.Raise(new DamagedEvent(targetEntityId, damage, damagingEntity.Id));
        }

        #endregion Private Methods
    }
}