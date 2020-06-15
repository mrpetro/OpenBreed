using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems
{
    public class TimerSystem : WorldSystem, ICommandExecutor, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<int> entities = new List<int>();

        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public TimerSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);
            Require<TimerComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(TimerStartCommand.TYPE, cmdHandler);
            World.RegisterHandler(TimerStopCommand.TYPE, cmdHandler);
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = Core.Entities.GetById(entities[i]);
                Debug.Assert(entity != null);

                Update(entity, dt);
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            cmdHandler.ExecuteEnqueued();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = Core.Entities.GetById(entities[i]);
                if (entity.Components.OfType<PauseImmuneComponent>().Any())
                    Update(entity, dt);
            }
        }

        private void Update(IEntity entity, float dt)
        {
            var tc = entity.GetComponent<TimerComponent>();

            //Update all timers with delta time
            for (int i = 0; i < tc.Items.Count; i++)
                UpdateTimer(entity, tc.Items[i], dt);
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

        private void RaiseTimerUpdateEvent(IEntity entity, TimerData timerData)
        {
            entity.RaiseEvent(new TimerUpdateEventArgs(timerData.TimerId));
        }

        private void RaiseTimerElapsedEvent(IEntity entity, TimerData timerData)
        {
            entity.RaiseEvent(new TimerElapsedEventArgs(timerData.TimerId));
        }

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TimerStartCommand.TYPE:
                    return HandleTimerStartCommandMsg((TimerStartCommand)cmd);

                case TimerStopCommand.TYPE:
                    return HandleTimerStopCommandMsg((TimerStopCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity.Id);
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTimerStartCommandMsg(TimerStartCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);

            var timerComponent = entity.GetComponent<TimerComponent>();

            if (timerComponent == null)
            {
                Core.Logging.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
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

        private bool HandleTimerStopCommandMsg(TimerStopCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);

            var timerComponent = entity.GetComponent<TimerComponent>();

            if (timerComponent == null)
            {
                Core.Logging.Warning($"Entity '{cmd.EntityId}' has missing Timer Component.");
                return false;
            }

            var timerData = timerComponent.Items.FirstOrDefault(item => item.TimerId == cmd.TimerId);

            if (timerData == null)
                return true;

            timerData.Enabled = false;

            return true;
        }


        #endregion Private Methods
    }
}
