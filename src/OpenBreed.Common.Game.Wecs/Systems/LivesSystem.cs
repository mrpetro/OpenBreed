using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System.Linq;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(LivesComponent))]
    public class LivesSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LivesSystem(
            IWorldMan worldMan,
            IEventsMan eventsMan,
            ILogger logger) : base(worldMan)
        {
            this.eventsMan = eventsMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var livesComponent = entity.Get<LivesComponent>();

            var previousLivesNo = livesComponent.Value;

            var toAdd = livesComponent.ToAdd;

            if (toAdd.Any())
            {
                for (int i = 0; i < toAdd.Count; i++)
                    livesComponent.Value += toAdd[i];

                toAdd.Clear();
            }

            var toRemove = livesComponent.ToRemove;

            if (toRemove.Any())
            {
                for (int i = 0; i < toRemove.Count; i++)
                    livesComponent.Value -= toRemove[i];

                toRemove.Clear();
            }

            if (previousLivesNo != livesComponent.Value)
                eventsMan.Raise(new LivesChangedEvent(entity.Id, livesComponent.Value));
        }

        #endregion Protected Methods
    }
}