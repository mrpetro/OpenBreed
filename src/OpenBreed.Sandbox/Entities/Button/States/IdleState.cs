using OpenBreed.Wecs.Systems.Rendering.Commands;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class IdleState : IState<ButtonState, ButtonImpulse>
    {
        public  const string NAME = "Idle";

        #region Private Fields


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
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, "NotUsedYet", 0));
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
            entity.Core.Commands.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Press));

        }

        #endregion Private Methods
    }
}