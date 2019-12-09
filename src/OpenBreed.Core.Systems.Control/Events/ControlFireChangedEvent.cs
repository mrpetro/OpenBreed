using System;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Events
{
    /// <summary>
    /// Event arguments that are passed with CONTROL_FIRE_CHANGED event
    /// </summary>
    public class ControlFireChangedEvent : EventArgs
    {
        #region Public Constructors

        public ControlFireChangedEvent(bool fire)
        {
            Fire = fire;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Fire { get; }

        #endregion Public Properties
    }
}