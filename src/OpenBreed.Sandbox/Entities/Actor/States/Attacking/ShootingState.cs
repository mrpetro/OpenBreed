using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Attacking
{
    public class ShootingState : IState<AttackingState, AttackingImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ProjectileHelper projectileHelper;

        #endregion Private Fields

        #region Public Constructors

        public ShootingState(IFsmMan fsmMan, ProjectileHelper projectileHelper)
        {
            this.fsmMan = fsmMan;
            this.projectileHelper = projectileHelper;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Shooting;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            var pos = entity.Get<PositionComponent>().Value;
            pos += new Vector2(8, 8);
            var direction = entity.Get<AngularPositionComponent>().Value;
            direction.Normalize();
            direction *= 100.0f;
            projectileHelper.AddProjectile(entity.WorldId, pos.X, pos.Y, direction.X, direction.Y);

            entity.SetState(FsmId, (int)AttackingImpulse.Wait);
        }

        public void LeaveState(IEntity entity)
        {

        }

        #endregion Public Methods

    }
}