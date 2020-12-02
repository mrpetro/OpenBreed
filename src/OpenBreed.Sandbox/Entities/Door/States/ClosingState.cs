using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

using OpenBreed.Core.Modules.Physics.Commands;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Core.Components;
using OpenBreed.Core.Commands;

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
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
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