using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Door.States;
using System;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState()
        {
            stampPrefix = "Tiles/Stamps";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Closed;

        /// <summary>
        /// TODO: Insecure, encapsulate this somehow
        /// </summary>
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.PostCommand(new SpriteOffCommand(entity.Id));

            var pos = entity.GetComponent<PositionComponent>();

            var className = entity.GetComponent<ClassComponent>().Name;
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            var stampId = entity.Core.Rendering.Stamps.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            //STAMP_DOOR_HORIZONTAL_CLOSED = $"{stampPrefix}/{className}/{stateName}";

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
            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.Open));
        }

        #endregion Private Methods
    }
}