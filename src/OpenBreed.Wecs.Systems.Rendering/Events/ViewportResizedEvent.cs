using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Rendering.Events
{
    /// <summary>
    /// Event args for event that occurs when rendering viewport was resized
    /// </summary>
    public class ViewportResizedEvent : EntityEvent
    {
        #region Public Constructors

        public ViewportResizedEvent(int entityId, float width, float height)
            : base(entityId)
        {
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Width { get; }
        public float Height { get; }

        #endregion Public Properties
    }
}