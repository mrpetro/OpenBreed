using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TimerSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TimerSystem(IEntityMan entityMan, IEventsMan eventsMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;

            RequireEntityWith<TimerComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var tc = entity.Get<TimerComponent>();

            RaiseUpdateEvent(entity);

            //Update all timers with delta time
            for (int i = 0; i < tc.Items.Count; i++)
                UpdateTimer(entity, tc.Items[i], context.Dt);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateTimer(Entity entity, TimerData timerData, float dt)
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

        private void RaiseUpdateEvent(Entity entity)
        {
            eventsMan.Raise(entity, new WorldUpdateEvent(entity.Id));
        }

        private void RaiseTimerUpdateEvent(Entity entity, TimerData timerData)
        {
            eventsMan.Raise(entity, new TimerUpdateEventArgs(entity.Id, timerData.TimerId));
        }

        private void RaiseTimerElapsedEvent(Entity entity, TimerData timerData)
        {
            eventsMan.Raise(entity, new TimerElapsedEventArgs(entity.Id, timerData.TimerId));
        }

        #endregion Private Methods
    }
}