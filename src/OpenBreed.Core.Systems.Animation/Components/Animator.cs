using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Animation.Components
{
    public class Animator<T> : IEntityComponent
    {
        #region Private Fields

        private readonly Dictionary<string, IAnimationData<T>> animations = new Dictionary<string, IAnimationData<T>>();

        private IAnimationData<T> currentAnimation;
        private float currentPosition;

        #endregion Private Fields

        #region Public Constructors

        public Animator(float speed = 1.0f, bool loop = false)
        {
            Speed = speed;
            Loop = loop;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Paused { get; set; }
        public float Speed { get; set; }
        public IEnumerable<string> AnimationNames { get { return animations.Keys; } }

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

        public T Frame { get; private set; }

        public void Animate(float dt)
        {
            if (Paused)
                return;

            if (currentAnimation == null)
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

            Frame = currentAnimation.GetFrame(currentPosition);
        }

        public void AddAnimation(string animationName, IAnimationData<T> animation)
        {
            animations.Add(animationName, animation);
        }

        #endregion Public Methods
    }
}