using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class FsmManExtensions
    {
        #region Public Methods

        public static void SetupButtonStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var triggerMan = serviceProvider.GetService<ITriggerMan>();

            var buttonFsm = fsmMan.Create<Entities.Button.ButtonState, Entities.Button.ButtonImpulse>("Button");

            buttonFsm.AddState(new Entities.Button.States.IdleState(fsmMan, triggerMan));
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

        public static void SetupActorAttackingStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var triggerMan = serviceProvider.GetService<ITriggerMan>();

            var projectileHelper = serviceProvider.GetService<ProjectileHelper>();

            var stateMachine = fsmMan.Create<Entities.Actor.States.Attacking.AttackingState, Entities.Actor.States.Attacking.AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new Entities.Actor.States.Attacking.ShootingState(fsmMan, projectileHelper));
            stateMachine.AddState(new Entities.Actor.States.Attacking.IdleState(fsmMan, triggerMan));
            stateMachine.AddState(new Entities.Actor.States.Attacking.CooldownState(fsmMan, triggerMan));

            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Shooting, Entities.Actor.States.Attacking.AttackingImpulse.Stop, Entities.Actor.States.Attacking.AttackingState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Shooting, Entities.Actor.States.Attacking.AttackingImpulse.Wait, Entities.Actor.States.Attacking.AttackingState.Cooldown);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Cooldown, Entities.Actor.States.Attacking.AttackingImpulse.Stop, Entities.Actor.States.Attacking.AttackingState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Cooldown, Entities.Actor.States.Attacking.AttackingImpulse.Shoot, Entities.Actor.States.Attacking.AttackingState.Shooting);
            stateMachine.AddTransition(Entities.Actor.States.Attacking.AttackingState.Idle, Entities.Actor.States.Attacking.AttackingImpulse.Shoot, Entities.Actor.States.Attacking.AttackingState.Shooting);
        }

        public static void SetupActorMovementStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var triggerMan = serviceProvider.GetService<ITriggerMan>();

            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();

            var stateMachine = fsmMan.Create<Entities.Actor.States.Movement.MovementState, Entities.Actor.States.Movement.MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new Entities.Actor.States.Movement.StandingState(fsmMan, clipMan, triggerMan));
            stateMachine.AddState(new Entities.Actor.States.Movement.WalkingState(fsmMan, clipMan, triggerMan));

            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Walking, Entities.Actor.States.Movement.MovementImpulse.Stop, Entities.Actor.States.Movement.MovementState.Standing);
            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Standing, Entities.Actor.States.Movement.MovementImpulse.Walk, Entities.Actor.States.Movement.MovementState.Walking);
            stateMachine.AddTransition(Entities.Actor.States.Movement.MovementState.Walking, Entities.Actor.States.Movement.MovementImpulse.Walk, Entities.Actor.States.Movement.MovementState.Walking);
        }


        public static void CreateTurretRotationStates(this IFsmMan fsmMan, IServiceProvider serviceProvider)
        {
            var clipMan = serviceProvider.GetService<IClipMan<Entity>>();
            var triggerMan = serviceProvider.GetService<ITriggerMan>();

            var stateMachine = fsmMan.Create<Entities.Actor.States.Rotation.RotationState, Entities.Actor.States.Rotation.RotationImpulse>("Turret.Rotation");

            stateMachine.AddState(new Entities.Actor.States.Rotation.IdleState(fsmMan, triggerMan));
            stateMachine.AddState(new Entities.Actor.States.Rotation.RotatingState(clipMan, fsmMan, triggerMan));

            stateMachine.AddTransition(Entities.Actor.States.Rotation.RotationState.Rotating, Entities.Actor.States.Rotation.RotationImpulse.Stop, Entities.Actor.States.Rotation.RotationState.Idle);
            stateMachine.AddTransition(Entities.Actor.States.Rotation.RotationState.Idle, Entities.Actor.States.Rotation.RotationImpulse.Rotate, Entities.Actor.States.Rotation.RotationState.Rotating);
        }

        #endregion Public Methods
    }
}