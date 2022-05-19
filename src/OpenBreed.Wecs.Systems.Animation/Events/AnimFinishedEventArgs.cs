using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Animation.Events
{
    /// <summary>
    /// Event args for event that occurs when animation is finished
    /// </summary>
    public class AnimFinishedEventArgs : EntityEvent
    {
        #region Public Constructors

        public AnimFinishedEventArgs(int entityId, Animator animator )
            : base(entityId)
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public Animator Animator { get; }

        #endregion Public Properties
    }
}