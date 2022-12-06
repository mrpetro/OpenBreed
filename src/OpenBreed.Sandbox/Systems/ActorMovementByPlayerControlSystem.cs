using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Attributes;
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
    [RequireEntityWith(
        typeof(WalkingInputComponent),
        typeof(ControlComponent))]
    internal class ActorMovementByPlayerControlSystem : UpdatableSystemBase<ActorMovementByPlayerControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly IPlayersMan playersMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorMovementByPlayerControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IPlayersMan playersMan) :
            base(world)
        {
            this.entityMan = entityMan;
            this.playersMan = playersMan;
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