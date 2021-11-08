using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Animation;
using System;

namespace OpenBreed.Wecs.Systems.Animation.Events
{
    /// <summary>
    /// Event args for event that occurs when animation is finished
    /// </summary>
    public class AnimFinishedEventArgs : EventArgs
    {
        #region Public Constructors

        public AnimFinishedEventArgs(Animator animator )
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public Animator Animator { get; }

        #endregion Public Properties
    }
}