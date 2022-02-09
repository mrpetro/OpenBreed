using System;

namespace OpenBreed.Wecs.Systems.Control.Events
{
    /// <summary>
    /// Event args for event that occurs when control fire flag changes
    /// </summary>
    public class ControlFireChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public ControlFireChangedEventArgs(bool fire)
        {
            Fire = fire;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Fire { get; }

        #endregion Public Properties
    }
}