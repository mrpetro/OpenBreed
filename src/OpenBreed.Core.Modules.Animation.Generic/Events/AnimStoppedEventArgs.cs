using OpenBreed.Core.Modules.Animation.Components;
using System;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Event arguments that are passed with ANIMATION_STOPPED event
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