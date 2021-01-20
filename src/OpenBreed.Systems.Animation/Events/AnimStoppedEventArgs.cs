using OpenBreed.Animation.Interface;
using System;

namespace OpenBreed.Systems.Animation.Events
{
    /// <summary>
    /// Event args for event that occurs when animation is stopped
    /// </summary>
    public class AnimStoppedEventArgs : EventArgs
    {
        #region Public Constructors

        public AnimStoppedEventArgs(IAnimator animator )
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public IAnimator Animator { get; }

        #endregion Public Properties
    }
}