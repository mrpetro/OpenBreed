using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public class CameraComponent : IEntityComponent
    {
        #region Public Constructors

        public CameraComponent(float zoom, float brightness)
        {
            Zoom = zoom;
            Brightness = brightness;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Zoom { get; set; }

        public float Brightness { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static CameraComponent Create(float zoom, float brightness = 1.0f)
        {
            return new CameraComponent(zoom, brightness);
        }

        #endregion Public Methods
    }
}