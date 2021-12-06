using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Wecs.Worlds;
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
        /// World from which rendering will start
        /// </summary>
        World ScreenWorld { get; set; }

        #endregion Public Properties
    }
}