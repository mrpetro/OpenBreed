using OpenBreed.Wecs.Events;
using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Rendering.Events
{
    /// <summary>
    /// Event args for event that occurs when rendering viewport was clicked with cursor
    /// </summary>
    public class ViewportClickedEventArgs : EntityEvent
    {
        #region Public Constructors

        public ViewportClickedEventArgs(int entityId, Vector2 location)
            : base(entityId)
        {
            Location = location;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Location { get; }

        #endregion Public Properties
    }
}