using OpenBreed.Rendering.Interface.Events;
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

        /// <summary>
        /// Render viewport
        /// </summary>
        /// <param name="drawBorder">Draw surrounding border</param>
        /// <param name="drawBackground">Draw background</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="viewportTransform">Viewport transformation</param>
        /// <param name="func">Drawing function</param>
        void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func);

        #endregion Public Properties
    }
}