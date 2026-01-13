using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Animation.Components;
using System;

namespace OpenBreed.Wecs.Animation.Systems.Events
{
    /// <summary>
    /// Event args for event that occurs when animation is finished
    /// </summary>
    public class AnimFinishedEvent : EntityEvent
    {
        #region Public Constructors

        public AnimFinishedEvent(int entityId, Animator animator )
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