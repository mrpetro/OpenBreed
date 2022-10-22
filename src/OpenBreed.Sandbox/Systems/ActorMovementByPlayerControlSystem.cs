using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Systems
{
    internal class ActorMovementByPlayerControlSystem : UpdatableSystemBase
    {
        private readonly IEntityMan entityMan;
        #region Private Fields

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
            RequireEntityWith<WalkingControlComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var controlComponent = entity.Get<WalkingInputComponent>();
            var walkingControl = entity.Get<WalkingControlComponent>();

            var player = playersMan.GetById(controlComponent.PlayerId);

            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input is null)
                return;

            if (!input.Changed)
                return;

            if (walkingControl.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(walkingControl.ControlledEntityId);

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

            //walkingControl.Direction = new Vector2(input.AxisX, input.AxisY);
            //entity.RaiseEvent(new ControlDirectionChangedEventArgs(walkingControl.Direction));
        }

        #endregion Protected Methods
    }
}
