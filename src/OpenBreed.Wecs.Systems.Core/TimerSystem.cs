using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TimerSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TimerSystem(IEntityMan entityMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.logger = logger;

            RequireEntityWith<TimerComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, float dt)
        {
            var tc = entity.Get<TimerComponent>();

            //Update all timers with delta time
            for (int i = 0; i < tc.Items.Count; i++)
                UpdateTimer(entity, tc.Items[i], dt);
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

        private void RaiseTimerUpdateEvent(Entity entity, TimerData timerData)
        {
            //eventQueue.Enqueue(new ComponentChangedEvent<TimerComponent>(entity.Id,

            entity.RaiseEvent(new TimerUpdateEventArgs(timerData.TimerId));
        }

        private void RaiseTimerElapsedEvent(Entity entity, TimerData timerData)
        {
            entity.RaiseEvent(new TimerElapsedEventArgs(timerData.TimerId));
        }

        #endregion Private Methods
    }
}