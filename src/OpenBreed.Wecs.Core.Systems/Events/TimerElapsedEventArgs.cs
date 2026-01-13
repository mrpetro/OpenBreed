using OpenBreed.Wecs.Abstractions.Events;
using System;

namespace OpenBreed.Wecs.Core.Systems.Events
{
    /// <summary>
    /// Event args for event that occurs when timer has elapsed
    /// </summary>
    public class TimerElapsedEventArgs : EntityEvent
    {
        #region Public Constructors

        public TimerElapsedEventArgs(int entityId, int timerId)
            : base(entityId)
        {
            TimerId = timerId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TimerId { get; }

        #endregion Public Properties
    }
}