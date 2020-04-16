using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class PressedState : IStateEx<ButtonState, ButtonImpulse>
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

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOffCommand(entity.Id));

            var pos = entity.GetComponent<PositionComponent>();
            entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));
            entity.PostCommand(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            entity.Subscribe<CollisionEventArgs>(OnCollision);
        }

        public void LeaveState(IEntity entity)
        {
            entity.Unsubscribe<CollisionEventArgs>(OnCollision);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, CollisionEventArgs eventArgs)
        {
            var entity = sender as IEntity;
            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Unpress));
        }

        #endregion Private Methods
    }
}