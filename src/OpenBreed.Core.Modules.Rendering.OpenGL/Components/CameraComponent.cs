namespace OpenBreed.Core.Modules.Rendering.Components
{
    public class CameraComponent : ICameraComponent
    {
        #region Public Constructors

        public CameraComponent(float zoom)
        {
            Zoom = zoom;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Zoom { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static ICameraComponent Create(float zoom)
        {
            return new CameraComponent(zoom);
        }

        #endregion Public Methods
    }
}