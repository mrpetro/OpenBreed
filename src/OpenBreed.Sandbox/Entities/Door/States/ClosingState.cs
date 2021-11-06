using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;
        private readonly IFsmMan fsmMan;
        private readonly IStampMan stampMan;
        private readonly ICommandsMan commandsMan;
        private readonly string stampPrefix;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(IFsmMan fsmMan, ICommandsMan commandsMan, IStampMan stampMan)
        {
            this.animPrefix = "Animations";
            this.stampPrefix = "Tiles/Stamps";
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Closing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.SetSpriteOn();
            entity.SetBodyOn();
            var pos = entity.Get<PositionComponent>();

            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            commandsMan.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}.{className}.{stateName}", 0));
            var stampId = stampMan.GetByName($"{stampPrefix}/{className}/Closed").Id;

            entity.SetText(0, "Door - Closing");
            entity.PutStamp(stampId, 0, pos.Value);

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.SetSpriteOff();

            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            var entity = sender as Entity;
            entity.SetState(FsmId, (int)FunctioningImpulse.StopClosing);
        }

        #endregion Public Methods

    }
}