using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities.Projectile.States
{
    public class FiredState : IState
    {
        #region Private Fields

        private ISprite sprite;
        private readonly string animPrefix;
        private IVelocity velocity;

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

            var direction = Entity.Components.OfType<IVelocity>().First().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);

            Entity.PostMsg(new PlayAnimMsg(Entity, animPrefix + animDirName));
            Entity.PostMsg(new TextSetMsg(Entity, "Projectile - Fired"));
            Entity.Subscribe(CollisionEvent.TYPE, OnCollision);

            Entity.Subscribe(AnimChangedEvent.TYPE, OnFrameChanged);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            velocity = entity.Components.OfType<IVelocity>().First();
            sprite = entity.Components.OfType<ISprite>().First();
        }

        public void LeaveState()
        {
            Entity.Unsubscribe(AnimChangedEvent.TYPE, OnFrameChanged);
        }

        private void OnFrameChanged(object sender, IEvent e)
        {
            HandleFrameChangeEvent((AnimChangedEvent)e);
        }

        private void HandleFrameChangeEvent(AnimChangedEvent systemEvent)
        {
            sprite.ImageId = (int)systemEvent.Frame;
        }

        public string Process(string actionName, object[] arguments)
        {

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCollision(object sender, IEvent e)
        {
            HandleCollisionEvent((CollisionEvent)e);
        }

        private void HandleCollisionEvent(CollisionEvent e)
        {
            //Entity.PostMsg(new StateChangeMsg(Entity, "Attacking", "Open"));
        }

        #endregion Private Methods
    }
}