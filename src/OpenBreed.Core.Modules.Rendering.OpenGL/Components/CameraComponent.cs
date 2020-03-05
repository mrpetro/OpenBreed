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

        private CameraComponent(float zoom, float brightness)
        {
            Zoom = zoom;
            Brightness = brightness;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Zoom value of camera
        /// </summary>
        public float Zoom { get; set; }

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
        public static CameraComponent Create(float zoom, float brightness = 1.0f)
        {
            return new CameraComponent(zoom, brightness);
        }

        #endregion Public Methods
    }
}