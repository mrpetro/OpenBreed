using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Animation
{
    public class AnimatorSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        private readonly IEntityMan entityMan;

        private readonly IClipMan<Entity> clipMan;

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimatorSystem(IEntityMan entityMan, IClipMan<Entity> clipMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.clipMan = clipMan;
            this.logger = logger;

            RequireEntityWith<AnimationComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].ComponentValues.OfType<PauseImmuneComponent>().Any())
                    Animate(entities[i], dt);
            }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Animate(entities[i], dt);
            }
        }

        public void Set(Entity entity, Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            animator.ClipId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Finish(Entity entity, Animator animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
            RaiseAnimFinishedEvent(entity, animator);
        }

        private void Animate(Entity entity, float dt)
        {
            var ac = entity.Get<AnimationComponent>();

            //Update all animators with delta time
            for (int i = 0; i < ac.States.Count; i++)
                UpdateAnimator(entity, ac.States[i], dt);
        }

        private void UpdateAnimator(Entity entity, Animator animator, float dt)
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

        private void RaiseAnimFinishedEvent(Entity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimFinishedEventArgs(animator));
        }

        #endregion Private Methods
    }
}