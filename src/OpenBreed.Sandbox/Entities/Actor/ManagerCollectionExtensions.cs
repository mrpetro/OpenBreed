using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Wecs;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupActorAttackingStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();
            var projectileHelper = managerCollection.GetManager<ProjectileHelper>();

            var stateMachine = fsmMan.Create<AttackingState, AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new States.Attacking.ShootingState(fsmMan, projectileHelper));
            stateMachine.AddState(new States.Attacking.IdleState(fsmMan, commandsMan));
            stateMachine.AddState(new States.Attacking.CooldownState(fsmMan, commandsMan));

            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Wait, AttackingState.Cooldown);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Shoot, AttackingState.Shooting);
            stateMachine.AddTransition(AttackingState.Idle, AttackingImpulse.Shoot, AttackingState.Shooting);
        }

        public static void SetupActorMovementStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();
            var clipMan = managerCollection.GetManager<IClipMan>();

            var stateMachine = fsmMan.Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState(fsmMan, commandsMan, clipMan));
            stateMachine.AddState(new WalkingState(fsmMan, commandsMan, clipMan));

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);
            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Walk, MovementState.Walking);
        }

        public static void SetupActorRotationStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<RotationState, RotationImpulse>("Actor.Rotation");

            stateMachine.AddState(new States.Rotation.IdleState(fsmMan, commandsMan));
            stateMachine.AddState(new States.Rotation.RotatingState(fsmMan, commandsMan));

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);
        }

        #endregion Public Methods
    }
}