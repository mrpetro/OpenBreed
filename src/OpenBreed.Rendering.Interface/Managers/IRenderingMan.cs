using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
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

        void Draw(float dt);

        void OnClientResized(float width, float height);

        void Subscribe<T>(Action<object, T> callback) where T : EventArgs;

        #endregion Public Methods
    }
}