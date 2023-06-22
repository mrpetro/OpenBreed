using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Wecs.Systems
{
    [RequireEntityWith(
        typeof(PlayerInputsComponent),
        typeof(ControllerComponent))]
    internal class ActorControlByPlayerInputsSystem : InputsEventSystem<ActorControlByPlayerInputsSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public ActorControlByPlayerInputsSystem(
            IInputsMan inputsMan,
            IEntityMan entityMan,
            IEventsMan eventsMan
            )
            : base(inputsMan)
        {
            this.inputsMan = inputsMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, KeyboardStateEventArgs e)
        {
            var playerInputsComponent = entity.Get<PlayerInputsComponent>();
            var controllerComponent = entity.Get<ControllerComponent>();

            if (controllerComponent.ControlledEntityId == -1)
                return;

            var controlledEntity = entityMan.GetById(controllerComponent.ControlledEntityId);

            var angularVelocity = controlledEntity.Get<AngularPositionTargetComponent>();

            var dx = 0.0f;
            var dy = 0.0f;

            if (inputsMan.IsPressed((int)playerInputsComponent.Right)) dx = 1.0f;
            else if (inputsMan.IsPressed((int)playerInputsComponent.Left)) dx = -1.0f;
            if (inputsMan.IsPressed((int)playerInputsComponent.Down)) dy = -1.0f;
            else if (inputsMan.IsPressed((int)playerInputsComponent.Up)) dy = 1.0f;

            var direction = new Vector2(dx, dy);

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

            if (inputsMan.IsPressed((int)playerInputsComponent.Fire))
                eventsMan.Raise(null, new EntityActionEvent<PlayerActions>(controlledEntity.Id, PlayerActions.Fire));

            if (inputsMan.IsPressed((int)playerInputsComponent.SwitchWeapon))
                eventsMan.Raise(null, new EntityActionEvent<PlayerActions>(controlledEntity.Id, PlayerActions.SwitchWeapon));

        }

        #endregion Protected Methods
    }
}