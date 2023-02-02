using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Systems
{
    [RequireEntityWith(
        typeof(ThrustControlComponent),
        typeof(ControllerComponent))]
    internal class ActorMovementByPlayerControlSystem : UpdatableSystemBase<ActorMovementByPlayerControlSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Internal Constructors

        internal ActorMovementByPlayerControlSystem(
            IWorld world,
            IEntityMan entityMan,
            IInputsMan inputsMan)
        {
            this.entityMan = entityMan;
            this.inputsMan = inputsMan;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var thrustControlComponent = entity.Get<ThrustControlComponent>();
            var controllerComponent = entity.Get<ControllerComponent>();

            var dx = 0.0f;
            var dy = 0.0f;
            if (inputsMan.IsPressed(thrustControlComponent.RightCode)) dx = 1.0f;
            else if (inputsMan.IsPressed(thrustControlComponent.LeftCode)) dx = -1.0f;
            if (inputsMan.IsPressed(thrustControlComponent.DownCode)) dy = -1.0f;
            else if (inputsMan.IsPressed(thrustControlComponent.UpCode)) dy = 1.0f;

            var direction = new Vector2(dx, dy);

            if (controllerComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controllerComponent.ControlledEntityId);

            var angularVelocity = controlledEntity.Get<AngularPositionTargetComponent>();

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