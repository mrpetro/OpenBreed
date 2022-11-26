using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Systems
{
    internal class ActorMovementByPlayerControlSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly IPlayersMan playersMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorMovementByPlayerControlSystem(
            IEntityMan entityMan,
            IPlayersMan playersMan)
        {
            this.entityMan = entityMan;
            this.playersMan = playersMan;

            RequireEntityWith<WalkingInputComponent>();
            RequireEntityWith<ControlComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var inputComponent = entity.Get<WalkingInputComponent>();
            var controlComponent = entity.Get<ControlComponent>();

            var player = playersMan.GetById(inputComponent.PlayerId);

            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            if (controlComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controlComponent.ControlledEntityId);

            var angularVelocity = controlledEntity.Get<AngularVelocityComponent>();

            var direction = new Vector2(input.AxisX, input.AxisY);

            if (direction != Vector2.Zero)
            {
                var movement = controlledEntity.Get<MotionComponent>();
                controlledEntity.Get<ThrustComponent>().Value = direction * movement.Acceleration;
                angularVelocity.Value = direction;
            }
            else
            {
                var thrust = controlledEntity.Get<ThrustComponent>();
                thrust.Value = Vector2.Zero;

                var angularPosition = controlledEntity.Get<AngularPositionComponent>();
                angularVelocity.Value = angularPosition.Value;
            }
        }

        #endregion Protected Methods
    }
}