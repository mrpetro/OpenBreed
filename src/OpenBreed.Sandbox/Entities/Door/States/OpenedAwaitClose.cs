using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Events;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedAwaitClose : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;

        #endregion Private Fields

        #region Public Constructors

        public OpenedAwaitClose()
        {
            this.stampPrefix = "Tiles/Stamps";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Opened;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOffCommand(entity.Id));
            entity.Core.Commands.Post(new BodyOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();

            //entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            var stampId = entity.Core.Rendering.Stamps.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Opened"));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Subscribe<TimerUpdateEventArgs>(OnTimerUpdate);
            entity.Core.Commands.Post(new TimerStartCommand(entity.Id, 0, 5.0));
        }

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.Close));
        }

        private void OnTimerUpdate(object sender, TimerUpdateEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var tcp = entity.Get<TimerComponent>();

            var timer = tcp.Items.FirstOrDefault(item => item.TimerId == 0);

            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, $"Door - {timer.Interval:F2}s"));
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<TimerUpdateEventArgs>(OnTimerUpdate);
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Core.Commands.Post(new TimerStopCommand(entity.Id, 0));
        }

        #endregion Public Methods

    }
}