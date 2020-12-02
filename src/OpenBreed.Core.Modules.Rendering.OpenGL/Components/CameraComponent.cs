using OpenBreed.Core.Components;
using OpenBreed.Core.Modules.Rendering.Builders;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Camera component as source of display for viewports
    /// Related systems:
    /// - ViewportSystem
    /// </summary>
    public class CameraComponent : IEntityComponent
    {
        #region Internal Constructors

        internal CameraComponent(CameraComponentBuilder builder)
        {
            Width = builder.Width;
            Height = builder.Height;
            Brightness = builder.Brightness;
        }

        #endregion Internal Constructors

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
        /// Aspect ratio of this camera
        /// </summary>
        public float Ratio { get { return Width / Height; } }

        /// <summary>
        /// Brightness value of camera
        /// </summary>
        public float Brightness { get; set; }

        #endregion Public Properties
    }
}