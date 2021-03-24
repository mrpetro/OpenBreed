using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Actor.States.Movement;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorMovementHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var fsmMan = core.GetManager<IFsmMan>();
            var commandsMan = core.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<MovementState, MovementImpulse>("Actor.Movement");

            stateMachine.AddState(new StandingState(fsmMan, commandsMan));
            stateMachine.AddState(new WalkingState(fsmMan, commandsMan));

            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Stop, MovementState.Standing);
            stateMachine.AddTransition(MovementState.Standing, MovementImpulse.Walk, MovementState.Walking);
            stateMachine.AddTransition(MovementState.Walking, MovementImpulse.Walk, MovementState.Walking);
        }

        #endregion Public Methods
    }
}