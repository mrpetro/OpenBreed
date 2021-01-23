﻿using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorMovementHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.GetManager<IFsmMan>().Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState());
            stateMachine.AddState(new WalkingState());

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);
            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Walk, MovementState.Walking);


            //stateMachine.AddOnEnterState(MovementState.Standing, MovementImpulse.Stop, OnMovementEnterStandingWithStop);
            //stateMachine.AddOnEnterState(MovementState.Walking, MovementImpulse.Walk, OnMovementEnterWalkingWithWalk);
            //stateMachine.AddOnLeaveState(MovementState.Standing, MovementImpulse.Walk, OnMovementLeaveStandingWithWalk);
            //stateMachine.AddOnLeaveState(MovementState.Walking, MovementImpulse.Stop, OnMovementLeaveWalkingWithStop);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnMovementLeaveWalkingWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnMovementEnterWalkingWithWalk(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            entity.Get<ThrustComponent>().Value = direction.GetDirection() * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.GetDirection());

            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(fsmId, (int)MovementState.Walking);
            var className = entity.Get<ClassComponent>().Name;
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{stateName}/{animDirPostfix}", 0));

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;
            var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Movement").Id;

            if (e.Direction != Vector2.Zero)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Walk));
            else
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Stop));
        }

        private static void OnMovementEnterStandingWithStop(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            Console.WriteLine("Enter Standing(Stop)");

            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            var direction = entity.Get<AngularPositionComponent>().GetDirection();

            var animDirName = AnimHelper.ToDirectionName(direction);
            var className = entity.Get<ClassComponent>().Name;

            var thrust = entity.Get<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(fsmId, (int)MovementState.Standing);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{stateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.GetManager<IFsmMan>().GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged2);
        }

        private static void OnMovementLeaveStandingWithWalk(ICore core, int entityId, int fsmId, int stateId, int withImpulseId)
        {
            Console.WriteLine("Leave Standing(Walk)");

            var entity = core.GetManager<IEntityMan>().GetById(entityId);

            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged2);
        }

        private static void OnControlDirectionChanged2(object sender, ControlDirectionChangedEventArgs eventArgs)
        {
            var entity = sender as Entity;

            var fsmId = entity.Core.GetManager<IFsmMan>().GetByName("Actor.Movement").Id;

            if (eventArgs.Direction != Vector2.Zero)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Walk));
        }

        #endregion Private Methods
    }
}