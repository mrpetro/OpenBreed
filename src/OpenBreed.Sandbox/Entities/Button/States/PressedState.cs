using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class PressedState : IState<ButtonState, ButtonImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;

        #endregion Private Fields

        #region Public Constructors

        public PressedState(IFsmMan fsmMan)
        {
            this.fsmMan = fsmMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Pressed;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.SetSpriteOff();

            var pos = entity.Get<PositionComponent>();

            entity.PutStampAtPosition(0, 0, pos.Value);
            //commandsMan.Post(new PutStampCommand(entity.Id, 0, 0, pos.Value));

            entity.SetText(0, "Door - Closed");

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