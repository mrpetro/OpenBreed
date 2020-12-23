﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Rendering.Systems.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class RotatingState : IState<RotationState, RotationImpulse>
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public RotatingState()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)RotationState.Rotating;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            //var direction = entity.Get<DirectionComponent>();
            //var movement = entity.Get<MotionComponent>();
            //entity.Get<ThrustComponent>().Value = direction.GetDirection() * movement.Acceleration;

            //var animDirName = AnimHelper.ToDirectionName(direction.GetDirection());
            //var className = entity.Get<ClassComponent>().Name;
            //var movementFsm = entity.Core.StateMachines.GetByName("Actor.Movement");
            //var movementStateName = movementFsm.GetCurrentStateName(entity);
            //entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{movementStateName}/{animDirName}", 0));

            //var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            //entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            //entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)RotationImpulse.Stop));
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods
    }
}