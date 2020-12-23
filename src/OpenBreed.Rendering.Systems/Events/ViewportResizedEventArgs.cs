using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Rendering.Systems.Events
{
    /// <summary>
    /// Event args for event that occurs when rendering viewport was resized
    /// </summary>
    public class ViewportResizedEventArgs : EventArgs
    {
        #region Public Constructors

        public ViewportResizedEventArgs(float width, float height)
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