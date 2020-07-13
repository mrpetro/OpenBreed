using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorMovementHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var stateMachine = core.StateMachines.Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState());
            stateMachine.AddState(new WalkingState());

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);

            stateMachine.AddOnEnterState(MovementState.Standing, MovementImpulse.Stop, OnMovementEnterStandingWithStop);
            stateMachine.AddOnEnterState(MovementState.Walking, MovementImpulse.Walk, OnMovementEnterWalkingWithWalk);
            stateMachine.AddOnLeaveState(MovementState.Standing, MovementImpulse.Walk, OnMovementLeaveStandingWithWalk);
            stateMachine.AddOnLeaveState(MovementState.Walking, MovementImpulse.Stop, OnMovementLeaveWalkingWithStop);
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnMovementLeaveWalkingWithStop(ICore core, int entityId)
        {
            var entity = core.Entities.GetById(entityId);

            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnMovementEnterWalkingWithWalk(ICore core, int entityId)
        {
            var entity = core.Entities.GetById(entityId);

            var direction = entity.Get<DirectionComponent>();
            var movement = entity.Get<MotionComponent>();
            entity.Get<ThrustComponent>().Value = direction.Value * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);

            var fsmId = entity.Core.StateMachines.GetByName("Actor.Movement").Id;
            var stateName = entity.Core.StateMachines.GetStateName(fsmId, (int)MovementState.Walking);
            var className = entity.Get<ClassComponent>().Name;
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{stateName}/{animDirPostfix}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        private static void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;
            var fsmId = entity.Core.StateMachines.GetByName("Actor.Movement").Id;

            if (e.Direction != Vector2.Zero)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Walk));
            else
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Stop));
        }

        private static void OnMovementEnterStandingWithStop(ICore core, int entityId)
        {
            var entity = core.Entities.GetById(entityId);

            var direction = entity.Get<DirectionComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);
            var className = entity.Get<ClassComponent>().Name;

            var thrust = entity.Get<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var fsmId = entity.Core.StateMachines.GetByName("Actor.Movement").Id;
            var stateName = entity.Core.StateMachines.GetStateName(fsmId, (int)MovementState.Standing);
            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, $"{"Animations"}/{className}/{stateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged2);
        }

        private static void OnMovementLeaveStandingWithWalk(ICore core, int entityId)
        {
            var entity = core.Entities.GetById(entityId);

            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged2);
        }

        private static void OnControlDirectionChanged2(object sender, ControlDirectionChangedEventArgs eventArgs)
        {
            var entity = sender as Entity;

            var fsmId = entity.Core.StateMachines.GetByName("Actor.Movement").Id;

            if (eventArgs.Direction != Vector2.Zero)
                entity.Core.Commands.Post(new SetStateCommand(entity.Id, fsmId, (int)MovementImpulse.Walk));
        }

        #endregion Private Methods
    }
}