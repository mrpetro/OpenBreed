using OpenBreed.Rendering.Interface.Events;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Rendering manager interface
    /// </summary>
    public interface IRenderingMan
    {
        #region Public Events

        /// <summary>
        /// Occurs when rendering view client is resized
        /// </summary>
        event EventHandler<ClientResizedEventArgs> ClientResized;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Get current rendering frames per second
        /// </summary>
        float Fps { get; }

        /// <summary>
        /// Object which will be rendered to client
        /// </summary>
        public IRenderableBatch Renderable { get; set; }

        #endregion Public Properties
    }
}