using System;

namespace OpenBreed.Systems.Control.Events
{
    /// <summary>
    /// Event args for event that occurs when control fire flag changes
    /// </summary>
    public class ControlFireChangedEvenrArgs : EventArgs
    {
        #region Public Constructors

        public ControlFireChangedEvenrArgs(bool fire)
        {
            Fire = fire;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Fire { get; }

        #endregion Public Properties
    }
}