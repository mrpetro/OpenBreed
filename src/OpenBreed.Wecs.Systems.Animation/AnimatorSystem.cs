using OpenBreed.Animation.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Animation
{
    [RequireEntityWith(typeof(AnimationComponent))]
    public class AnimatorSystem : UpdatableSystemBase<AnimatorSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly IClipMan<IEntity> clipMan;

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimatorSystem(
            IWorld world,
            IEntityMan entityMan,
            IClipMan<IEntity> clipMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.clipMan = clipMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Set(IEntity entity, Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            animator.ClipId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var ac = entity.Get<AnimationComponent>();

            //Update all animators with delta time
            for (int i = 0; i < ac.States.Count; i++)
                UpdateAnimator(entity, ac.States[i], context.Dt);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Finish(IEntity entity, Animator animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
            RaiseAnimFinishedEvent(entity, animator);
        }

        private void UpdateAnimator(IEntity entity, Animator animator, float dt)
        {
            if (animator.Paused)
                return;

            if (animator.ClipId < 0)
                return;

            var data = clipMan.GetById(animator.ClipId);

            animator.Position += animator.Speed * dt;

            if (animator.Position > data.Length)
            {
                if (animator.Loop)
                    animator.Position = animator.Position - data.Length;
                else
                {
                    Finish(entity, animator);
                    return;
                }
            }

            data.UpdateWithNextFrame(entity, animator.Position);
        }

        private void RaiseAnimFinishedEvent(IEntity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimFinishedEventArgs(entity.Id, animator));
        }

        #endregion Private Methods
    }
}