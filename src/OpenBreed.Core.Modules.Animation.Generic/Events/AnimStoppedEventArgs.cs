using OpenBreed.Core.Modules.Animation.Components;
using System;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Event args for event that occurs when animation is stopped
    /// </summary>
    public class AnimStoppedEventArgs : EventArgs
    {
        #region Public Constructors

        public AnimStoppedEventArgs(AnimationComponent animator)
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public AnimationComponent Animator { get; }

        #endregion Public Properties
    }
}