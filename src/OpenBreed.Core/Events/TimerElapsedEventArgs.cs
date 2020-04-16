using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when timer has elapsed
    /// </summary>
    public class TimerElapsedEventArgs : EventArgs
    {
        #region Public Constructors

        public TimerElapsedEventArgs(int timerId)
        {
            TimerId = timerId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TimerId { get; }

        #endregion Public Properties
    }
}