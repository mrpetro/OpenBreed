using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class PressedState : IState<ButtonState, ButtonImpulse>
    {
        #region Private Fields

        private readonly int stampId;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public PressedState(IFsmMan fsmMan, ICommandsMan commandsMan)
        {
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Pressed;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            commandsMan.Post(new SpriteOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();
            commandsMan.Post(new PutStampCommand(entity.Id, stampId, 0, pos.Value));
            commandsMan.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            //entity.Subscribe<CollisionEventArgs>(OnCollision);
        }

        public void LeaveState(Entity entity)
        {
            //entity.Unsubscribe<CollisionEventArgs>(OnCollision);
        }

        #endregion Public Methods

        //private void OnCollision(object sender, CollisionEventArgs eventArgs)
        //{
        //    var entity = sender as Entity;
        //    entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Unpress));
        //}
    }
}