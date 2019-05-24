using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation.Components
{
    public class SpriteAnimator : IAnimationComponent
    {
        #region Private Fields

        private ISprite sprite;
        private readonly Dictionary<string, Animation<int>> animations = new Dictionary<string, Animation<int>>();

        private Animation<int> currentAnimation;
        private float currentPosition;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAnimator(float speed = 1.0f, bool loop = false)
        {
            Speed = speed;
            Loop = loop;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Paused { get; set; }
        public float Speed { get; set; }
        public IEnumerable<string> AnimationNames { get { return animations.Keys; } }
        public Type SystemType { get { return typeof(AnimationSystem); } }

        public bool Loop { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Set(string animationName)
        {
            if (!animations.TryGetValue(animationName, out currentAnimation))
                throw new InvalidOperationException($"Animation '{animationName}' doesn't exist.");
        }

        public void Play(string animationName = null, float startPosition = 0.0f)
        {
            if (animationName != null)
                Set(animationName);

            currentPosition = startPosition;
            Paused = false;
        }

        public void Stop()
        {
            currentPosition = 0.0f;
            Paused = true;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Animate(float dt)
        {
            if (Paused)
                return;

            currentPosition += Speed * dt;

            if (currentPosition > currentAnimation.Length)
            {
                if (Loop)
                    currentPosition = currentPosition - currentAnimation.Length;
                else
                {
                    Stop();
                    return;
                }
            }

            sprite.ImageId = currentAnimation.GetFrame(currentPosition);
        }

        public void Deinitialize(IEntity entity)
        {
        }

        public void Initialize(IEntity entity)
        {
            sprite = entity.Components.OfType<ISprite>().First();
        }

        public void AddAnimation(string animationName, Animation<int> animation)
        {
            animations.Add(animationName, animation);
        }

        #endregion Public Methods
    }
}