using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState()
        {
            this.animPrefix = "Animations";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Closing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOnCommand(entity.Id));
            entity.Core.Commands.Post(new BodyOnCommand(entity.Id));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(FsmId, Id);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{animPrefix}/{className}/{stateName}", 0));


            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Closing"));
            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs e)
        {
            var entity = sender as Entity;
            entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.StopClosing));
        }

        #endregion Public Methods

    }
}