using OpenBreed.Rendering.Abstractions.Events;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Abstractions.Managers
{
    public enum MatrixMode
    {
        ModelView,
        Projection,
        Texture,
        Color
    }

    /// <summary>
    /// Rendering manager interface
    /// </summary>
    public interface IRenderingMan
    {
        #region Public Events

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Get current rendering frames per second
        /// </summary>
        float Fps { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the state
        /// </summary>
        /// <param name="dt">delta time</param>
        void Update(float dt);

        #endregion Public Methods
    }
}