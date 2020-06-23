﻿using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when timer has elapsed
    /// </summary>
    public class TimerUpdateEventArgs : EventArgs
    {
        #region Public Constructors

        public TimerUpdateEventArgs(int timerId)
        {
            TimerId = timerId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TimerId { get; }

        #endregion Public Properties
    }
}