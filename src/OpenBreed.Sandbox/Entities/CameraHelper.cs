using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Sandbox.Entities
{
    public class CameraHelper
    {
        #region Public Fields

        public const string CAMERA_FADE_OUT = "Vanilla/Common/Camera/Effects/FadeOut";

        public const string CAMERA_FADE_IN = "Vanilla/Common/Camera/Effects/FadeIn";

        #endregion Public Fields

        #region Private Fields

        private readonly IClipMan clipMan;
        private readonly IFrameUpdaterMan frameUpdaterMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public CameraHelper(IClipMan clipMan, IFrameUpdaterMan frameUpdaterMan, IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.clipMan = clipMan;
            this.frameUpdaterMan = frameUpdaterMan;
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateAnimations()
        {
            frameUpdaterMan.Register("Camera.Brightness", (FrameUpdater<float>)OnFrameUpdate);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnFrameUpdate(Entity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }

        public Entity CreateCamera(float x, float y, float width, float height)
        {
            var entity = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Camera.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetParameter("width", width)
                .SetParameter("height", height)
                .Build();

            return entity;
        }

        #endregion Private Methods
    }
}