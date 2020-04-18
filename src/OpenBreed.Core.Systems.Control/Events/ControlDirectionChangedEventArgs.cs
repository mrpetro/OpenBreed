
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Events
{
    /// <summary>
    /// Event args for event that occurs when control direction is changed
    /// </summary>
    public class ControlDirectionChangedEventArgs : EventArgs
    {
        #region Public Constructors

        public ControlDirectionChangedEventArgs(Vector2 direction)
        {
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}