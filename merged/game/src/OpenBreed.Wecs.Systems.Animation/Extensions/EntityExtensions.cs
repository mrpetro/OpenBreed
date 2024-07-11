using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class EntityExtensions
    {
        public static void PlayAnimation(this IEntity entity, int animatorId, int clipId = -1)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.ClipId = clipId;
            animator.Paused = false;
        }

        public static void PlayAnimation(this IEntity entity, int animatorId, int clipId = -1, float startPosition = 0.0f)
        {
            var ac = entity.Get<AnimationComponent>();
            var animator = ac.States[animatorId];
            animator.ClipId = clipId;
            animator.Paused = false;
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
            animator.Paused = true;
        }
    }
}
