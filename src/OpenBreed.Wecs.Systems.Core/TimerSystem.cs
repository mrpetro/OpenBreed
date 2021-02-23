﻿using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;
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

        #endregion Private Fields

        #region Internal Constructors

        internal TimerSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
            Require<TimerComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<TimerStartCommand>(HandleTimerStartCommand);
            commands.Register<TimerStopCommand>(HandleTimerStopCommand);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entityMan.GetById(entities[i]);
                Debug.Assert(entity != null);

                Update(entity, dt);
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
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

        private static bool HandleTimerStartCommand(ICore core, TimerStartCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var timerComponent = entity.Get<TimerComponent>();

            if (timerComponent == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
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

        private static bool HandleTimerStopCommand(ICore core, TimerStopCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var timerComponent = entity.Get<TimerComponent>();

            if (timerComponent == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
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
            entity.RaiseEvent(new TimerUpdateEventArgs(timerData.TimerId));
        }

        private void RaiseTimerElapsedEvent(Entity entity, TimerData timerData)
        {
            entity.RaiseEvent(new TimerElapsedEventArgs(timerData.TimerId));
        }

        #endregion Private Methods
    }
}