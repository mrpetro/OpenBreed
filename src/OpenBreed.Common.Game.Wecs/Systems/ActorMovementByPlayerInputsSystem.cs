using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Common.Game;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    [RequireEntityWith(
        typeof(PlayerInputsComponent),
        typeof(ControllerComponent))]
    internal class ActorMovementByPlayerInputsSystem : InputsEventSystem<ActorMovementByPlayerInputsSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Public Constructors

        public ActorMovementByPlayerInputsSystem(
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

            if (controllerComponent.ControlledEntityId == WecsConsts.NO_ENTITY_ID)
                return;

            var controlledEntity = entityMan.GetById(controllerComponent.ControlledEntityId);

            var angularVelocity = controlledEntity.Get<AngularVelocityComponent>();

            var dx = 0.0f;
            var dy = 0.0f;

            if (inputsMan.IsKeyPressed((int)playerInputsComponent.Right)) dx = 1.0f;
            else if (inputsMan.IsKeyPressed((int)playerInputsComponent.Left)) dx = -1.0f;
            if (inputsMan.IsKeyPressed((int)playerInputsComponent.Down)) dy = -1.0f;
            else if (inputsMan.IsKeyPressed((int)playerInputsComponent.Up)) dy = 1.0f;

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

            if (inputsMan.IsKeyPressed((int)playerInputsComponent.Fire))
                eventsMan.Raise(new EntityActionEvent<PlayerActions>(controlledEntity.Id, PlayerActions.Fire));

            if (inputsMan.IsKeyPressed((int)playerInputsComponent.SwitchWeapon))
                eventsMan.Raise(new EntityActionEvent<PlayerActions>(controlledEntity.Id, PlayerActions.SwitchWeapon));

        }

        #endregion Protected Methods
    }
}