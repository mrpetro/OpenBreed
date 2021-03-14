using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Actor.States.Attacking;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorAttackingHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var fsmMan = core.GetManager<IFsmMan>();
            var commandsMan = core.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<AttackingState, AttackingImpulse>("Actor.Attacking");

            stateMachine.AddState(new States.Attacking.ShootingState(fsmMan, commandsMan));
            stateMachine.AddState(new States.Attacking.IdleState(fsmMan, commandsMan));
            stateMachine.AddState(new States.Attacking.CooldownState(fsmMan, commandsMan));

            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Shooting, AttackingImpulse.Wait, AttackingState.Cooldown);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Stop, AttackingState.Idle);
            stateMachine.AddTransition(AttackingState.Cooldown, AttackingImpulse.Shoot, AttackingState.Shooting);
            stateMachine.AddTransition(AttackingState.Idle, AttackingImpulse.Shoot, AttackingState.Shooting);
        }

        #endregion Public Methods
    }
}