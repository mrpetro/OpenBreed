using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using System.Linq;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(LivesComponent))]
    public class LivesSystem : UpdatableMatchingSystemBase<LivesSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LivesSystem(
            IEventsMan eventsMan,
            ILogger logger)
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
                eventsMan.Raise(entity, new LivesChangedEvent(entity.Id, livesComponent.Value));
        }

        #endregion Protected Methods
    }
}