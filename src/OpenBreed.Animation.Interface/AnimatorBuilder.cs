using OpenBreed.Animation.Interface;
using OpenBreed.Core;
using System;

namespace OpenBreed.Animation.Interface
{
    public class AnimatorBuilder
    {
        #region Internal Fields

        internal float Speed { get; private set; }
        internal bool Loop { get; private set; }
        internal int AnimId { get; private set; }
        internal float Position { get; private set; }
        internal bool Paused { get; private set; }
        internal FrameInterpolation Transition { get; private set; }

        #endregion Internal Fields

        #region Protected Constructors

        internal AnimatorBuilder()
        {
            AnimId = -1;
        }

        #endregion Protected Constructors

        #region Public Methods

        public static AnimatorBuilder New()
        {
            return new AnimatorBuilder();
        }

        public Animator Build()
        {
            return new Animator(this);
        }

        public AnimatorBuilder SetSpeed(float speed)
        {
            Speed = speed;
            return this;
        }

        public AnimatorBuilder SetLoop(bool loop)
        {
            Loop = loop;
            return this;
        }

        public AnimatorBuilder SetPaused(bool paused)
        {
            Paused = paused;
            return this;
        }

        public AnimatorBuilder SetAnimId(int animId)
        {
            AnimId = animId;
            return this;
        }

        public AnimatorBuilder SetTransition(FrameInterpolation transition)
        {
            Transition = transition;
            return this;
        }

        #endregion Public Methods
    }
}