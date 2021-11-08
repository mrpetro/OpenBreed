using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Projectile.States;

namespace OpenBreed.Sandbox.Entities.Projectile
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupProjectileStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var clipMan = managerCollection.GetManager<IClipMan>();

            var stateMachine = fsmMan.Create<AttackingState, AttackingImpulse>("Projectile");
            stateMachine.AddState(new FiredState("Animations/Laser/Fired/", clipMan));
        }

        #endregion Public Methods
    }
}