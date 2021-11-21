using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Sandbox.Entities.Camera
{
    public class CameraHelper
    {
        #region Public Fields

        public const string CAMERA_FADE_OUT = "Animations/Camera/Effects/FadeOut";

        public const string CAMERA_FADE_IN = "Animations/Camera/Effects/FadeIn";

        #endregion Public Fields

        #region Private Fields

        private readonly IClipMan clipMan;

        private readonly IFrameUpdaterMan frameUpdaterMan;

        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public CameraHelper(IClipMan clipMan, IFrameUpdaterMan frameUpdaterMan, IDataLoaderFactory dataLoaderFactory)
        {
            this.clipMan = clipMan;
            this.frameUpdaterMan = frameUpdaterMan;
            this.dataLoaderFactory = dataLoaderFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateAnimations()
        {
            frameUpdaterMan.Register("Camera.Brightness", (FrameUpdater<float>)OnFrameUpdate);

            var animationLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader>();

            var cameraFadeOut = animationLoader.Load("Animations/Camera/Effects/FadeOut");
            var cameraFadeIn = animationLoader.Load("Animations/Camera/Effects/FadeIn");
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }

        #endregion Private Methods
    }
}