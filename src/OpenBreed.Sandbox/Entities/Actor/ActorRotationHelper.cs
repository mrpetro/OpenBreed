﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.CodeDom;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorRotationHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<RotationState, RotationImpulse>("Actor.Rotation");

            stateMachine.AddState(new States.Rotation.IdleState());
            stateMachine.AddState(new States.Rotation.RotatingState());

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);

            //stateMachine.AddOnEnterState(RotationState.Idle, RotationImpulse.Stop, OnRotationEnterIdleWithStop);
            //stateMachine.AddOnEnterState(RotationState.Rotating, RotationImpulse.Rotate, OnRotationEnterRotatingWithRotate);
            //stateMachine.AddOnLeaveState(RotationState.Idle, RotationImpulse.Rotate, OnRotationLeaveIdleWithRotate);
            //stateMachine.AddOnLeaveState(RotationState.Rotating, RotationImpulse.Stop, OnRotationLeaveRotatingWithStop);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnRotationLeaveRotatingWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.Entities.GetById(entityId);
            Console.WriteLine("Rotation (!Rotating -> Stop)");
        }

        private static void OnRotationEnterRotatingWithRotate(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.Entities.GetById(entityId);
            var direction = entity.Get<DirectionComponent>();
            var movement = entity.Get<MotionComponent>();
            entity.Get<ThrustComponent>().Value = direction.GetDirection() * movement.Acceleration;

            var animDirName = AnimHelper.ToDirectionName(direction.GetDirection());
            var className = entity.Get<ClassComponent>().Name;
            var movementFsm = entity.Core.StateMachines.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{movementStateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)RotationImpulse.Stop));
        }

        private static void OnRotationEnterIdleWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.Entities.GetById(entityId);

            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnRotationLeaveIdleWithRotate(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.Entities.GetById(entityId);
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }


        private static void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                var dir = entity.Get<DirectionComponent>();

                if (dir.GetDirection() != e.Direction)
                {
                    dir.SetDirection(e.Direction);
                    var fsmId = entity.Core.StateMachines.GetByName("Actor.Rotation").Id;
                    entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)RotationImpulse.Rotate));
                }
            }
        }


        #endregion Private Methods
    }
}