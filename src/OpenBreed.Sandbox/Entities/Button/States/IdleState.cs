
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

        public int Id => (int)ButtonState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOnCommand(entity.Id));
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, animationId, 0));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            var entity = sender as Entity;
            entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Press));

        }

        #endregion Private Methods
    }
}