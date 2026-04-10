using OpenBreed.Wecs.Control.Components;

namespace OpenBreed.Wecs.Control.Systems.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetControlledEntity(this IEntity entity, int controlledEntityId)
        {
            entity.Get<ControllerComponent>().ControlledEntityId = controlledEntityId;
        }

        public static int GetControlledEntityId(this IEntity entity)
        {
            return entity.Get<ControllerComponent>().ControlledEntityId;
        }

        public static void AnimationToEnd(this IEntity entity, int animatorId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.Position = 0.0f;
            animator.Speed = 0.0f;
        }

        public static void AnimationToBegin(this IEntity entity, int animatorId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.Position = 0.0f;
            animator.Speed = 0.0f;
        }

        public static void PlayAnimationEx(this IEntity entity, int animatorId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.Paused = false;
            animator.Speed = 1.0f;
        }

        public static void PlayAnimation(this IEntity entity, int animatorId, int clipId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.ClipId = clipId;
            animator.Paused = false;
            animator.Speed = 1.0f;
        }

        public static void PlayAnimation(this IEntity entity, int animatorId, int clipId, float startPosition)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.ClipId = clipId;
            animator.Paused = false;
            animator.Speed = 1.0f;
            animator.Position = startPosition;
        }

        public static void PauseAnimation(this IEntity entity, int animatorId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.Paused = true;
        }

        public static void StopAnimation(this IEntity entity, int animatorId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.Position = 0.0f;
            animator.Speed = 0.0f;
        }

        public static void SetAnimationClipById(this IEntity entity, int animatorId, int clipId)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.ClipId = clipId;
        }

        #endregion Public Methods
    }
}