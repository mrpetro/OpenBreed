using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Actor.States.Rotation;

namespace OpenBreed.Sandbox.Entities.Turret
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void CreateTurretRotationStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();

            var stateMachine = fsmMan.Create<RotationState, RotationImpulse>("Turret.Rotation");

            stateMachine.AddState(new Actor.States.Rotation.IdleState(fsmMan, commandsMan));
            stateMachine.AddState(new Actor.States.Rotation.RotatingState(fsmMan, commandsMan));

            stateMachine.AddTransition(RotationState.Rotating, RotationImpulse.Stop, RotationState.Idle);
            stateMachine.AddTransition(RotationState.Idle, RotationImpulse.Rotate, RotationState.Rotating);
        }

        #endregion Public Methods
    }
}