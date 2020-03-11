
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Projectile.States
{
    public class FiredState : IState
    {
        #region Private Fields

        private SpriteComponent sprite;
        private readonly string animPrefix;
        private VelocityComponent velocity;

        #endregion Private Fields

        #region Public Constructors

        public FiredState(string name, string animPrefix)
        {
            Name = name;
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {

            var direction = Entity.GetComponent<VelocityComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            Entity.PostCommand(new PlayAnimCommand(Entity.Id, animPrefix + animDirName));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, "Projectile - Fired"));
            Entity.Subscribe(PhysicsEventTypes.COLLISION_OCCURRED, OnCollision);

            Entity.Subscribe(AnimationEventTypes.ANIMATION_CHANGED, OnFrameChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            velocity = entity.GetComponent<VelocityComponent>();
            sprite = entity.GetComponent<SpriteComponent>();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimationEventTypes.ANIMATION_CHANGED, OnFrameChanged);
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            HandleFrameChangeEvent((AnimChangedEventArgs)e);
        }

        private void HandleFrameChangeEvent(AnimChangedEventArgs systemEvent)
        {
            sprite.ImageId = (int)systemEvent.Frame;
        }

        public string Process(string actionName, object[] arguments)
        {

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, EventArgs e)
        {
            HandleCollisionEvent((CollisionEventArgs)e);
        }

        private void HandleCollisionEvent(CollisionEventArgs e)
        {
            //Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Open"));
        }

        #endregion Private Methods
    }
}