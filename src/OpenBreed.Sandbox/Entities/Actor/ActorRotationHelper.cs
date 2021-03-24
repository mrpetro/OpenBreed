using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public static class ActorRotationHelper
    {
        #region Public Methods

        public static void CreateFsm(ICore core)
        {
            var fsmMan = core.GetManager<IFsmMan>();
            var commandsMan = core.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<RotationState, RotationImpulse>("Actor.Rotation");

            stateMachine.AddState(new States.Rotation.IdleState(fsmMan, commandsMan));
            stateMachine.AddState(new States.Rotation.RotatingState(fsmMan, commandsMan));

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);
        }

        #endregion Public Methods
    }
}