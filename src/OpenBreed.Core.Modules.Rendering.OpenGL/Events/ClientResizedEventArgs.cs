using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with CLIENT_RESIZED event
    /// </summary>
    public class ClientResizedEventArgs : EventArgs
    {
        #region Public Constructors

        public ClientResizedEventArgs(float width, float height)
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