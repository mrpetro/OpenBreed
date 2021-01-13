using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Systems.Rendering.Commands;
using OpenBreed.Core.States;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class PressedState : IState<ButtonState, ButtonImpulse>
    {
        #region Private Fields

        private readonly int stampId;

        #endregion Private Fields

        #region Public Constructors

        public PressedState()
        {
            //Name = id;
            //this.stampId = stampId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Pressed;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.Core.Commands.Post(new SpriteOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            //entity.Subscribe<CollisionEventArgs>(OnCollision);
        }

        public void LeaveState(Entity entity)
        {
            //entity.Unsubscribe<CollisionEventArgs>(OnCollision);
        }

        #endregion Public Methods

        #region Private Methods

        //private void OnCollision(object sender, CollisionEventArgs eventArgs)
        //{
        //    var entity = sender as Entity;
        //    entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Unpress));
        //}

        #endregion Private Methods
    }
}