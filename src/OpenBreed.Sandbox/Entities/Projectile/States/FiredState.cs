using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;

namespace OpenBreed.Sandbox.Entities.Projectile.States
{
    public class FiredState : IState<AttackingState, AttackingImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string animPrefix)
        {
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)AttackingState.Fired;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var direction = entity.Get<VelocityComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            entity.Core.Commands.Post(new PlayAnimCommand(entity.Id, animPrefix + animDirName, 0));
            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Projectile - Fired"));
            entity.Subscribe<CollisionEventArgs>(OnCollision);
        }

        public void Initialize(Entity entity)
        {
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, CollisionEventArgs e)
        {
            //Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Open"));
        }

        #endregion Private Methods
    }
}