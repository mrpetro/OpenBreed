using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when timer has elapsed
    /// </summary>
    public class TimerUpdateEventArgs : EntityEvent
    {
        #region Public Constructors

        public TimerUpdateEventArgs(int entityId, int timerId)
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