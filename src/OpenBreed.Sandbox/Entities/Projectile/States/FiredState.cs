using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Entities.Projectile.States
{
    public class FiredState : IState<AttackingState, AttackingImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string animPrefix, ICommandsMan commandsMan)
        {
            this.animPrefix = animPrefix;
            this.commandsMan = commandsMan;
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

            commandsMan.Post(new PlayAnimCommand(entity.Id, animPrefix + animDirName, 0));
            commandsMan.Post(new TextSetCommand(entity.Id, 0, "Projectile - Fired"));
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