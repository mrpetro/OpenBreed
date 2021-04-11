using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Commands;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedAwaitClose : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenedAwaitClose(IFsmMan fsmMan, ICommandsMan commandsMan, IStampMan stampMan)
        {
            this.stampPrefix = "Tiles/Stamps";
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Opened;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            commandsMan.Post(new SpriteOffCommand(entity.Id));
            commandsMan.Post(new BodyOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();

            //entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            commandsMan.Post(new PutStampCommand(entity.Id, stampId, 0, pos.Value));
            commandsMan.Post(new TextSetCommand(entity.Id, 0, "Door - Opened"));

            entity.Subscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            entity.Subscribe<TimerUpdateEventArgs>(OnTimerUpdate);
            commandsMan.Post(new TimerStartCommand(entity.Id, 0, 5.0));
        }

        private void OnTimerElapsed(object sender, TimerElapsedEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            commandsMan.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.Close));
        }

        private void OnTimerUpdate(object sender, TimerUpdateEventArgs e)
        {
            if (e.TimerId != 0)
                return;

            var entity = sender as Entity;

            var tcp = entity.Get<TimerComponent>();

            var timer = tcp.Items.FirstOrDefault(item => item.TimerId == 0);

            commandsMan.Post(new TextSetCommand(entity.Id, 0, $"Door - {timer.Interval:F2}s"));
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<TimerUpdateEventArgs>(OnTimerUpdate);
            entity.Unsubscribe<TimerElapsedEventArgs>(OnTimerElapsed);
            commandsMan.Post(new TimerStopCommand(entity.Id, 0));
        }

        #endregion Public Methods

    }
}