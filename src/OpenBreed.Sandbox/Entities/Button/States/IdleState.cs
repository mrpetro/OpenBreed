
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using System;
using System.Linq;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class IdleState : IState<ButtonState, ButtonImpulse>
    {
        public  const string NAME = "Idle";

        #region Private Fields

        private readonly string animationId;

        #endregion Private Fields

        #region Public Constructors

        public IdleState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public ButtonState Id => ButtonState.Idle;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostCommand(new SpriteOnCommand(Entity.Id));
            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animationId));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Door - Opening"));

            Entity.Subscribe<AnimChangedEventArgs>(OnAnimChanged);
            Entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<AnimChangedEventArgs>(OnAnimChanged);
            Entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public ButtonState Process(ButtonImpulse impulse, object[] arguments)
        {
            switch (impulse)
            {
                case ButtonImpulse.Press:
                    return ButtonState.Pressed;
                default:
                    break;
            }

            return Id;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimChanged(object sender, AnimChangedEventArgs e)
        {
            Entity.PostCommand(new SpriteSetCommand(Entity.Id, (int)e.Frame));
        }

        private void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "FunctioningState", "Opened"));
        }

        #endregion Private Methods
    }
}