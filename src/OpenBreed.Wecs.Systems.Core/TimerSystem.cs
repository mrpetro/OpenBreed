using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TimerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
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

        #region Public Methods

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Update(entities[i], dt);
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                if (entity.ComponentValues.OfType<PauseImmuneComponent>().Any())
                    Update(entity, dt);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Update(Entity entity, float dt)
        {
            var tc = entity.Get<TimerComponent>();

            //Update all timers with delta time
            for (int i = 0; i < tc.Items.Count; i++)
                UpdateTimer(entity, tc.Items[i], dt);
        }

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