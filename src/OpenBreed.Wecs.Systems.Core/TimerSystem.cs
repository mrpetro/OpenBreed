using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TimerSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<int> entities = new List<int>();
        private readonly IEntityMan entityMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TimerSystem(IEntityMan entityMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.logger = logger;

            RequireEntityWith<TimerComponent>();
            RegisterHandler<TimerStartCommand>(HandleTimerStartCommand);
            RegisterHandler<TimerStopCommand>(HandleTimerStopCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Update(float dt)
        {
            ExecuteCommands();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entityMan.GetById(entities[i]);
                Debug.Assert(entity != null);

                Update(entity, dt);
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entityMan.GetById(entities[i]);
                if (entity.Components.OfType<PauseImmuneComponent>().Any())
                    Update(entity, dt);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity.Id);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTimerStartCommand(TimerStartCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            var timerComponent = entity.Get<TimerComponent>();

            if (timerComponent == null)
            {
                logger.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
                return false;
            }

            var timerData = timerComponent.Items.FirstOrDefault(item => item.TimerId == cmd.TimerId);

            if (timerData == null)
            {
                timerData = new TimerData(cmd.TimerId, cmd.Interval);
                timerComponent.Items.Add(timerData);
            }
            else
                timerData.Interval = cmd.Interval;

            timerData.Enabled = true;

            return true;
        }

        private bool HandleTimerStopCommand(TimerStopCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);

            var timerComponent = entity.Get<TimerComponent>();

            if (timerComponent == null)
            {
                logger.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
                return false;
            }

            var timerData = timerComponent.Items.FirstOrDefault(item => item.TimerId == cmd.TimerId);

            if (timerData == null)
                return true;

            timerData.Enabled = false;

            return true;
        }

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