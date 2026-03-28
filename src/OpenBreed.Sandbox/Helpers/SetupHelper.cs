using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Hud;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Animation.Systems;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems;
using OpenBreed.Wecs.Scripting.Systems;
using OpenTK;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Helpers
{
    public class SetupHelper
    {
        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly IFontMan fontMan;
        private readonly IWindow viewClient;
        private readonly IEntityMan entityMan;
        private readonly ITriggerMan triggerMan;
        private readonly IEntityFactory entityFactory;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly IFrameUpdaterMan<IEntity> frameUpdaterMan;

        #endregion Private Fields

        #region Public Constructors

        public SetupHelper(
            ISystemFactory systemFactory,
            IWorldMan worldMan,
            IFontMan fontMan,
            IWindow viewClient,
            IEntityMan entityMan,
            ITriggerMan triggerMan,
            IEntityFactory entityFactory,
            IRepositoryProvider repositoryProvider,
            IPaletteMan paletteMan,
            PalettesDataProvider palettesDataProvider,
            IDataLoaderFactory dataLoaderFactory,
            ITextureMan textureMan,
            ISpriteMan spriteMan,
            IFrameUpdaterMan<IEntity> frameUpdaterMan)
        {
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.fontMan = fontMan;
            this.viewClient = viewClient;

            this.entityMan = entityMan;
            this.triggerMan = triggerMan;
            this.entityFactory = entityFactory;
            this.repositoryProvider = repositoryProvider;
            this.paletteMan = paletteMan;
            this.palettesDataProvider = palettesDataProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.frameUpdaterMan = frameUpdaterMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Setup()
        {
            CreateNonDbAssets();
            BindAnimationProperties();
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateNonDbAssets()
        {
            var statusBarTexture = textureMan.Create(
            "StatusBarPixel",
            1,
            1,
            new byte[] { 0x4D });

            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbStatusBarSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById("Vanilla/Common/Status");

            //var colors = paletteModel.GetColors(0, 64);

            //paletteModel.SetColors(64, colors);

            var spriteAtlas = loader.Load(dbStatusBarSpriteAtlas);

            //Create FontAtlas
            var fontAtlas = fontMan.Create()
                                     .SetName("StatusBar")
                                     .SetAlias("Gfx/StatusBar")
                                     .SetSpriteAtlas("Vanilla/Common/Status")
                                     .MapCharacterToSpriteId('0', 2)
                                     .MapCharacterToSpriteId('1', 3)
                                     .MapCharacterToSpriteId('2', 4)
                                     .MapCharacterToSpriteId('3', 5)
                                     .MapCharacterToSpriteId('4', 6)
                                     .MapCharacterToSpriteId('5', 7)
                                     .MapCharacterToSpriteId('6', 8)
                                     .MapCharacterToSpriteId('7', 9)
                                     .MapCharacterToSpriteId('8', 10)
                                     .MapCharacterToSpriteId('9', 11)
                                     .MapCharacterToSpriteId('+', 12)
                                     .Build();

            var healthBarPixel = spriteMan.CreateAtlas()
                .SetName("StatusBarPixel")
                .SetTexture(statusBarTexture.Id)
                .AppendCoord(0, 0, 1, 1)
                .Build();
        }

        private void BindAnimationProperties()
        {
            frameUpdaterMan.Register("Camera.Brightness", (FrameUpdater<IEntity, float>)UpdateCameraBrightness);
            frameUpdaterMan.Register("Text.Color.A", (FrameUpdater<IEntity, float>)UpdateTextColorA);
            frameUpdaterMan.Register("Picture.Color.R", (FrameUpdater<IEntity, float>)UpdatePictureColorR);
            frameUpdaterMan.Register("Picture.Color.G", (FrameUpdater<IEntity, float>)UpdatePictureColorG);
            frameUpdaterMan.Register("Picture.Color.B", (FrameUpdater<IEntity, float>)UpdatePictureColorB);
        }

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

        #endregion Private Methods
    }
}