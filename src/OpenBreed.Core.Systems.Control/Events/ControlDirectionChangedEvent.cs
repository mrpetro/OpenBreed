
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Events
{
    /// <summary>
    /// Event arguments that are passed with CONTROL_DIRECTION_CHANGED event
    /// </summary>
    public class ControlDirectionChangedEvent : EventArgs
    {
        #region Public Constructors

        public ControlDirectionChangedEvent(Vector2 direction)
        {
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}