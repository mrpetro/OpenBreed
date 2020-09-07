using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimatorBuilder
    {
        #region Internal Fields

        internal float Speed { get; private set; }
        internal bool Loop { get; private set; }
        internal int AnimId { get; private set; }
        internal float Position { get; private set; }
        internal bool Paused { get; private set; }
        internal FrameTransition Transition { get; private set; }

        #endregion Internal Fields

        #region Protected Constructors

        internal AnimatorBuilder(ICore core)
        {
            AnimId = -1;
        }

        #endregion Protected Constructors

        #region Public Methods

        public static AnimatorBuilder New(ICore core)
        {
            return new AnimatorBuilder(core);
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

        public AnimatorBuilder SetTransition(FrameTransition transition)
        {
            Transition = transition;
            return this;
        }

        #endregion Public Methods
    }
}