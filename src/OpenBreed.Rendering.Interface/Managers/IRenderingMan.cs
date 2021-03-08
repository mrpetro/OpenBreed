using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Rendering manager interface
    /// </summary>
    public interface IRenderingMan
    {
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

        #region Public Methods

        void Cleanup();

        void Subscribe<T>(Action<object, T> callback) where T : EventArgs;

        #endregion Public Methods
    }
}