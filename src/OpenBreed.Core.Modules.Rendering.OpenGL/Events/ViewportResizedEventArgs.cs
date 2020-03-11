using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with VIEWPORT_RESIZED event
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