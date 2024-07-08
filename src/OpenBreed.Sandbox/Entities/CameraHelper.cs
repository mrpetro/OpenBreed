using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities
{
    public class CameraHelper
    {
        #region Public Fields

        public const string CAMERA_FADE_OUT = "Vanilla/Common/Camera/Effects/FadeOut";

        public const string CAMERA_FADE_IN = "Vanilla/Common/Camera/Effects/FadeIn";

        #endregion Public Fields

        #region Private Fields

        private readonly IClipMan<IEntity> clipMan;
        private readonly IFrameUpdaterMan<IEntity> frameUpdaterMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly ITriggerMan triggerMan;
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public CameraHelper(
            IClipMan<IEntity> clipMan,
            IFrameUpdaterMan<IEntity> frameUpdaterMan,
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            ITriggerMan triggerMan,
            IEntityMan entityMan,
            IWorldMan worldMan,
            ILogger logger)
        {
            this.clipMan = clipMan;
            this.frameUpdaterMan = frameUpdaterMan;
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.triggerMan = triggerMan;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateAnimations()
        {
            frameUpdaterMan.Register("Camera.Brightness", (FrameUpdater<IEntity, float>)UpdateCameraBrightness);
            frameUpdaterMan.Register("Text.Color.A", (FrameUpdater<IEntity, float>)UpdateTextColorA);
            frameUpdaterMan.Register("Picture.Color.R", (FrameUpdater<IEntity, float>)UpdatePictureColorR);
            frameUpdaterMan.Register("Picture.Color.G", (FrameUpdater<IEntity, float>)UpdatePictureColorG);
            frameUpdaterMan.Register("Picture.Color.B", (FrameUpdater<IEntity, float>)UpdatePictureColorB);
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateCameraBrightness(IEntity entity, float nextValue)
        {
            var cameraCmp = entity.Get<CameraComponent>();
            cameraCmp.Brightness = nextValue;
        }

        private void UpdateTextColorA(IEntity entity, float nextValue)
        {
            var textCmp = entity.Get<TextComponent>();
            var c = textCmp.Parts[0].Color;
            textCmp.Parts[0].Color = new OpenTK.Mathematics.Color4(c.R, c.G, c.B, nextValue);
        }

        private void UpdatePictureColorA(IEntity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, c.G, c.B, nextValue);
        }

        private void UpdatePictureColorR(IEntity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(nextValue, c.G, c.B, c.A);
        }

        private void UpdatePictureColorG(IEntity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, nextValue, c.B, c.A);
        }

        private void UpdatePictureColorB(IEntity entity, float nextValue)
        {
            var picCmp = entity.Get<PictureComponent>();
            var c = picCmp.Color;
            picCmp.Color = new OpenTK.Mathematics.Color4(c.R, c.G, nextValue, c.A);
        }

        public IEntity CreateCamera(string name, float x, float y, float width, float height)
        {
            var camera = entityFactory.Create(@"ABTA\Templates\Common\Camera")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetParameter("width", width)
                .SetParameter("height", height)
                .SetTag(name)
            .Build();

            triggerMan.OnEntityEnteredWorld(camera, (e, args) =>
            {
                var world = worldMan.GetById(camera.WorldId);

                var paletteEntityTag = $"Palettes/{world.Name}";

                var paletteEntity = entityMan.GetByTag(paletteEntityTag).FirstOrDefault();

                if (paletteEntity is null)
                {
                    logger.LogError("Unable to set palette '{0}' on camera '{1}'.", paletteEntityTag, camera.Tag);
                    return;
                }

                var pid = paletteEntity.GetPaletteId();
                camera.SetPaletteId(pid);

                logger.LogTrace("Palette '{0}' set on camera '{1}'.", paletteEntityTag, camera.Tag);

            }, singleTime: false);


            return camera;
        }

        #endregion Private Methods
    }
}