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

        public AnimStoppedEventArgs(AnimatorComponent animator)
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public AnimatorComponent Animator { get; }

        #endregion Public Properties
    }
}