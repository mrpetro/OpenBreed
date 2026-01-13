using Microsoft.Extensions.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Core.Systems.Events;
using System;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(typeof(TimerComponent))]
    [SystemCategory(CommonCategories.General)]
    public class TimerSystem : IMatchingSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TimerSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var tc = entity.Get<TimerComponent>();

            //Update all timers with delta time
            for (int i = 0; i < tc.Items.Count; i++)
                UpdateTimer(entity, tc.Items[i], context.Dt);
        }

        private void UpdateTimer(IEntity entity, TimerData timerData, float dt)
        {
            if (!timerData.Enabled)
                return;

            timerData.Interval -= dt;

            if (timerData.Interval > 0.0)
            {
                RaiseTimerUpdateEvent(entity, timerData);
                return;
            }

            timerData.Enabled = false;
            RaiseTimerElapsedEvent(entity, timerData);
        }

        private void RaiseUpdateEvent(IEntity entity)
        {
            eventsMan.Raise(new EntityFrameEvent(entity.Id));
        }

        private void RaiseTimerUpdateEvent(IEntity entity, TimerData timerData)
        {
            eventsMan.Raise(new TimerUpdateEventArgs(entity.Id, timerData.TimerId));
        }

        private void RaiseTimerElapsedEvent(IEntity entity, TimerData timerData)
        {
            eventsMan.Raise(new TimerElapsedEventArgs(entity.Id, timerData.TimerId));
        }

        #endregion Private Methods
    }
}