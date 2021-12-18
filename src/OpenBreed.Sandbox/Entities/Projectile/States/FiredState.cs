using OpenBreed.Wecs.Components.Common;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Sandbox.Entities.Projectile.States
{
    public class FiredState : IState<AttackingState, AttackingImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;
        private readonly IClipMan<Entity> clipMan;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string animPrefix, IClipMan<Entity> clipMan)
        {
            this.animPrefix = animPrefix;
            this.clipMan = clipMan;
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

            var clipId = clipMan.GetByName(animPrefix + animDirName).Id;

            entity.PlayAnimation(0, clipId);
            //commandsMan.Post(new PlayAnimCommand(entity.Id, animPrefix + animDirName, 0));

            entity.SetText(0, "Projectile - Fired");
        }

        public void Initialize(Entity entity)
        {
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}