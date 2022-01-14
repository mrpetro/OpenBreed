using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Fsm;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class FsmManExtensions
    {
        #region Public Methods

        public static void SetupButtonStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var buttonFsm = fsmMan.Create<Entities.Button.ButtonState, Entities.Button.ButtonImpulse>("Button");

            buttonFsm.AddState(new Entities.Button.States.IdleState(fsmMan));
            buttonFsm.AddState(new Entities.Button.States.PressedState(fsmMan));

            buttonFsm.AddTransition(Entities.Button.ButtonState.Pressed, Entities.Button.ButtonImpulse.Unpress, Entities.Button.ButtonState.Idle);
            buttonFsm.AddTransition(Entities.Button.ButtonState.Idle, Entities.Button.ButtonImpulse.Press, Entities.Button.ButtonState.Pressed);
        }

        public static void SetupProjectileStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();

            var stateMachine = fsmMan.Create<Entities.Projectile.States.AttackingState, Entities.Projectile.States.AttackingImpulse>("Projectile");
            stateMachine.AddState(new Entities.Projectile.States.FiredState("Animations/Laser/Fired/", clipMan));
        }

        public static void SetupDoorStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var collisionMan = serviceProvider.GetService<ICollisionMan<Entity>>();
            var stampMan = serviceProvider.GetService<IStampMan>();
            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();
            var soundMan = serviceProvider.GetService<ISoundMan>();
            var itemsMan = serviceProvider.GetService<ItemsMan>();

            var fsm = fsmMan.Create<Entities.Door.States.FunctioningState, Entities.Door.States.FunctioningImpulse>("Door.Functioning");

            fsm.AddState(new Components.States.OpeningState(fsmMan, stampMan, clipMan, soundMan));
            //fsm.AddState(new Components.States.OpenedAwaitClose(fsmMan, stampMan));
            fsm.AddState(new Components.States.OpenedState(fsmMan, stampMan));
            fsm.AddState(new Components.States.ClosingState(fsmMan, stampMan, clipMan));
            fsm.AddState(new Components.States.ClosedState(fsmMan, collisionMan, stampMan, itemsMan));

            fsm.AddTransition(Entities.Door.States.FunctioningState.Closed, Entities.Door.States.FunctioningImpulse.Open, Entities.Door.States.FunctioningState.Opening);
            fsm.AddTransition(Entities.Door.States.FunctioningState.Opening, Entities.Door.States.FunctioningImpulse.StopOpening, Entities.Door.States.FunctioningState.Opened);
            fsm.AddTransition(Entities.Door.States.FunctioningState.Opened, Entities.Door.States.FunctioningImpulse.Close, Entities.Door.States.FunctioningState.Closing);
            fsm.AddTransition(Entities.Door.States.FunctioningState.Closing, Entities.Door.States.FunctioningImpulse.StopClosing, Entities.Door.States.FunctioningState.Closed);
        }

        public static void SetupPickableStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var collisionMan = serviceProvider.GetService<ICollisionMan<Entity>>();
            var stampMan = serviceProvider.GetService<IStampMan>();
            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();
            var soundMan = serviceProvider.GetService<ISoundMan>();
            var itemsMan = serviceProvider.GetService<ItemsMan>();

            var fsm = fsmMan.Create<Entities.Pickable.States.FunctioningState, Entities.Pickable.States.FunctioningImpulse>("Pickable.Functioning");

            fsm.AddState(new Entities.Pickable.States.LyingState(fsmMan, collisionMan, stampMan, itemsMan));
            fsm.AddState(new Entities.Pickable.States.PickedState(fsmMan, stampMan, soundMan));

            fsm.AddTransition(Entities.Pickable.States.FunctioningState.Lying, Entities.Pickable.States.FunctioningImpulse.Pick, Entities.Pickable.States.FunctioningState.Picked);
        }


        public static void SetupActorAttackingStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var projectileHelper = serviceProvider.GetService<ProjectileHelper>();

            var stateMachine = fsmMan.Create<Entities.Actor.States.Attacking.AttackingState, Entities.Actor.States.Attacking.AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new Entities.Actor.States.Attacking.ShootingState(fsmMan, projectileHelper));
            stateMachine.AddState(new Entities.Actor.States.Attacking.IdleState(fsmMan));
            stateMachine.AddState(new Entities.Actor.States.Attacking.CooldownState(fsmMan));

            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Shooting, Entities.Actor.States.Attacking.AttackingImpulse.Stop, Entities.Actor.States.Attacking.AttackingState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Shooting, Entities.Actor.States.Attacking.AttackingImpulse.Wait, Entities.Actor.States.Attacking.AttackingState.Cooldown);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Cooldown, Entities.Actor.States.Attacking.AttackingImpulse.Stop, Entities.Actor.States.Attacking.AttackingState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Cooldown, Entities.Actor.States.Attacking.AttackingImpulse.Shoot, Entities.Actor.States.Attacking.AttackingState.Shooting);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Idle, Entities.Actor.States.Attacking.AttackingImpulse.Shoot, Entities.Actor.States.Attacking.AttackingState.Shooting);
        }

        public static void SetupActorMovementStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();

            var stateMachine = fsmMan.Create<Entities.Actor.States.Movement.MovementState, Entities.Actor.States.Movement.MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new Entities.Actor.States.Movement.StandingState(fsmMan, clipMan));
            stateMachine.AddState(new Entities.Actor.States.Movement.WalkingState(fsmMan, clipMan));

            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Walking, Entities.Actor.States.Movement.MovementImpulse.Stop, Entities.Actor.States.Movement.MovementState.Standing);
            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Standing, Entities.Actor.States.Movement.MovementImpulse.Walk, Entities.Actor.States.Movement.MovementState.Walking);
            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Walking, Entities.Actor.States.Movement.MovementImpulse.Walk, Entities.Actor.States.Movement.MovementState.Walking);
        }


        public static void CreateTurretRotationStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var stateMachine = fsmMan.Create<Entities.Actor.States.Rotation.RotationState, Entities.Actor.States.Rotation.RotationImpulse>("Turret.Rotation");

            stateMachine.AddState(new Entities.Actor.States.Rotation.IdleState(fsmMan));
            stateMachine.AddState(new Entities.Actor.States.Rotation.RotatingState(fsmMan));

            stateMachine.AddTransition(Entities.Actor.States.Rotation.RotationState.Rotating, Entities.Actor.States.Rotation.RotationImpulse.Stop, Entities.Actor.States.Rotation.RotationState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Rotation.RotationState.Idle, Entities.Actor.States.Rotation.RotationImpulse.Rotate, Entities.Actor.States.Rotation.RotationState.Rotating);
        }

        #endregion Public Methods
    }
}