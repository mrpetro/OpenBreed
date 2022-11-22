using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
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

        private readonly IClipMan<Entity> clipMan;
        private readonly IFrameUpdaterMan<Entity> frameUpdaterMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public CameraHelper(IClipMan<Entity> clipMan, IFrameUpdaterMan<Entity> frameUpdaterMan, IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
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
            frameUpdaterMan.Register("Camera.Brightness", (FrameUpdater<Entity, float>)UpdateCameraBrightness);
            frameUpdaterMan.Register("Text.Color.A", (FrameUpdater<Entity, float>)UpdateTextColorA);
            frameUpdaterMan.Register("Picture.Color.R", (FrameUpdater<Entity, float>)UpdatePictureColorR);
            frameUpdaterMan.Register("Picture.Color.G", (FrameUpdater<Entity, float>)UpdatePictureColorG);
            frameUpdaterMan.Register("Picture.Color.B", (FrameUpdater<Entity, float>)UpdatePictureColorB);
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateCameraBrightness(Entity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }

        private void UpdateTextColorA(Entity entity, float nextValue)
        {
            var textCmp = entity.Get<TextComponent>();
            var c = textCmp.Parts[0].Color;
            textCmp.Parts[0].Color = new OpenTK.Mathematics.Color4(c.R, c.G, c.B, nextValue);
        }

        private void UpdatePictureColorA(Entity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, c.G, c.B, nextValue);
        }

        private void UpdatePictureColorR(Entity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(nextValue, c.G, c.B, c.A);
        }

        private void UpdatePictureColorG(Entity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, nextValue, c.B, c.A);
        }

        private void UpdatePictureColorB(Entity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, c.G, nextValue, c.A);
        }

        public Entity CreateCamera(string name, float x, float y, float width, float height)
        {
            var entity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\Camera.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetParameter("width", width)
                .SetParameter("height", height)
                .SetTag(name)
                .Build();

            return entity;
        }

        #endregion Private Methods
    }
}