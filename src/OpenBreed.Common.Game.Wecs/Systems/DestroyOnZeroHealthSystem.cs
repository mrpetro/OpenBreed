using Microsoft.Extensions.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(HealthComponent))]
    public class DestroyOnZeroHealthSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public DestroyOnZeroHealthSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger) : base(worldMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var healthComponent = entity.Get<HealthComponent>();

            if (healthComponent.Value > 0)
            {
                return;
            }

            eventsMan.Raise(new DestroyedEvent(entity.Id));
        }

        #endregion Protected Methods
    }
}