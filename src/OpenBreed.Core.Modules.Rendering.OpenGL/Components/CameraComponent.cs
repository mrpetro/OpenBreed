using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Camera component as source of display for viewports
    /// Related systems:
    /// - ViewportSystem
    /// </summary>
    public class CameraComponent : IEntityComponent
    {
        #region Private Constructors

        private CameraComponent(float width, float height, float brightness)
        {
            Width = width;
            Height = height;
            Brightness = brightness;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Width value of camera
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height value of camera
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Brightness value of camera
        /// </summary>
        public float Brightness { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates camera component for viewport system
        /// </summary>
        /// <param name="zoom">Initial zoom value</param>
        /// <param name="brightness">Initial brightness value</param>
        /// <returns>Camera component</returns>
        public static CameraComponent Create(float zoom, float height, float brightness = 1.0f)
        {
            return new CameraComponent(zoom, height, brightness);
        }

        #endregion Public Methods
    }
}