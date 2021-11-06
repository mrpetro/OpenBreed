using OpenBreed.Animation.Interface;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Animation
{
    public class AnimationSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        private readonly IEntityMan entityMan;

        private readonly IClipMan clipMan;

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationSystem(IEntityMan entityMan, IClipMan clipMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.clipMan = clipMan;
            this.logger = logger;

            RequireEntityWith<AnimationComponent>();
            RegisterHandler<PlayAnimCommand>(HandlePlayAnimCommand);
            RegisterHandler<PauseAnimCommand>(HandlePauseAnimCommand);
            RegisterHandler<StopAnimCommand>(HandleStopAnimCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].ComponentValues.OfType<PauseImmuneComponent>().Any())
                    Animate(entities[i], dt);
            }
        }

        public void Update(float dt)
        {
            ExecuteCommands();

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

        public void Play(Entity entity, Animator animator, int animId = -1, float startPosition = 0.0f)
        {
            if (animId != -1)
                animator.ClipId = animId;

            animator.ClipId = animId;
            animator.Position = startPosition;
            animator.Paused = false;
        }

        public void Pause(Entity entity, Animator animator)
        {
            animator.Paused = true;
        }

        public void Stop(Entity entity, Animator animator)
        {
            animator.Position = 0.0f;
            animator.Paused = true;
            RaiseAnimStoppedEvent(entity, animator);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool DenqueueCommand(IEntityCommand entityCommand)
        {
            return base.DenqueueCommand(entityCommand);
        }

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

        private bool HandlePauseAnimCommand(PauseAnimCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[cmd.AnimatorId];

            Pause(entity, animator);

            return true;
        }

        private bool HandleStopAnimCommand(StopAnimCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[cmd.AnimatorId];

            Stop(entity, animator);

            return true;
        }

        private bool HandlePlayAnimCommand(PlayAnimCommand cmd)
        {
            var entity = entityMan.GetById(cmd.EntityId);
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[cmd.AnimatorId];

            int animId = -1;

            if (cmd.Id != null)
            {
                var data = clipMan.GetByName(cmd.Id);

                if (data == null)
                    logger.Warning($"Animation with ID = '{cmd.Id}' not found.");
                else
                    animId = data.Id;
            }
            else
            {
                animId = animator.ClipId;
            }

            Play(entity, animator, animId);

            return true;
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
                    Stop(entity, animator);
                    return;
                }
            }

            data.UpdateWithNextFrame(entity, animator.Position);
        }

        private void RaiseAnimStoppedEvent(Entity entity, Animator animator)
        {
            entity.RaiseEvent(new AnimStoppedEventArgs(animator));
        }

        #endregion Private Methods
    }
}