using OpenTK;
using System;

namespace OpenBreed.Systems.Rendering.Events
{
    /// <summary>
    /// Event args for event that occurs when rendering viewport was clicked with cursor
    /// </summary>
    public class ViewportClickedEventArgs : EventArgs
    {
        #region Public Constructors

        public ViewportClickedEventArgs(Vector2 location)
        {
            Location = location;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Location { get; }

        #endregion Public Properties
    }
}