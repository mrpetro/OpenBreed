using OpenBreed.Core.Common.Systems.Components;
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
        private SpriteComponent sprite;
        private VelocityComponent velocity;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string animPrefix)
        {
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public AttackingState Id => AttackingState.Fired;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            var direction = Entity.GetComponent<VelocityComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animPrefix + animDirName));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Projectile - Fired"));
            Entity.Subscribe<CollisionEventArgs>(OnCollision);

            Entity.Subscribe<AnimChangedEventArgs>(OnFrameChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            velocity = entity.GetComponent<VelocityComponent>();
            sprite = entity.GetComponent<SpriteComponent>();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe<AnimChangedEventArgs>(OnFrameChanged);
        }

        public AttackingState Process(AttackingImpulse impulse, object[] arguments)
        {
            return Id;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameChanged(object sender, AnimChangedEventArgs e)
        {
            sprite.ImageId = (int)e.Frame;
        }

        private void OnCollision(object sender, CollisionEventArgs e)
        {
            //Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Open"));
        }

        #endregion Private Methods
    }
}