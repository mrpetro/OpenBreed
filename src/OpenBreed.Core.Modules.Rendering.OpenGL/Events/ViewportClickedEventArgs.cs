using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with VIEWPORT_CLICKED event
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