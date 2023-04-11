using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class GameHudWorldHelper
    {
        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly IWorldMan worldMan;
        private readonly IFontMan fontMan;
        private readonly IViewClient viewClient;
        private readonly IEntityMan entityMan;
        private readonly ITriggerMan triggerMan;
        private readonly IEntityFactory entityFactory;
        private readonly VanillaStatusBarHelper hudHelper;
        private readonly CameraHelper cameraHelper;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public GameHudWorldHelper(
            ISystemFactory systemFactory, 
            IRenderableFactory renderableFactory,
            IWorldMan worldMan, 
            IFontMan fontMan, 
            IViewClient viewClient, 
            IEntityMan entityMan,
            ITriggerMan triggerMan,
            IEntityFactory entityFactory,
            VanillaStatusBarHelper hudHelper, 
            CameraHelper cameraHelper, 
            IRepositoryProvider repositoryProvider, 
            IPaletteMan paletteMan,
            PalettesDataProvider palettesDataProvider,
            IDataLoaderFactory dataLoaderFactory,
            SpriteAtlasDataProvider spriteAtlasDataProvider,
            ITextureMan textureMan,
            ISpriteMan spriteMan)
        {
            this.systemFactory = systemFactory;
            this.renderableFactory = renderableFactory;
            this.worldMan = worldMan;
            this.fontMan = fontMan;
            this.viewClient = viewClient;

            this.entityMan = entityMan;
            this.triggerMan = triggerMan;
            this.entityFactory = entityFactory;
            this.hudHelper = hudHelper;
            this.cameraHelper = cameraHelper;
            this.repositoryProvider = repositoryProvider;
            this.paletteMan = paletteMan;
            this.palettesDataProvider = palettesDataProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
        }

        #endregion Public Constructors

        #region Public Methods


        private void AddPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            var paletteEntity = entityMan.Create(tag: $"GameHUD/Palette/Main");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray());

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

            worldMan.RequestAddEntity(paletteEntity, world.Id);
        }


        public void Create()
        {
            var statusBarTexture = textureMan.Create(
            "StatusBarPixel",
            1,
            1,
            new byte[] { 0x4D } );


            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbStatusBarSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById("Vanilla/Common/Status");

            var spriteSet = spriteAtlasDataProvider.GetSpriteSet(dbStatusBarSpriteAtlas.Id);

            //var colors = paletteModel.GetColors(0, 64);

            //paletteModel.SetColors(64, colors);

            var spriteAtlas = loader.Load(dbStatusBarSpriteAtlas.Id);

            //Create FontAtlas
            var fontAtlas = fontMan.Create()
                                     .SetName("StatusBar")
                                     .AddCharacterFromSprite('0', "Vanilla/Common/Status", 2)
                                     .AddCharacterFromSprite('1', "Vanilla/Common/Status", 3)
                                     .AddCharacterFromSprite('2', "Vanilla/Common/Status", 4)
                                     .AddCharacterFromSprite('3', "Vanilla/Common/Status", 5)
                                     .AddCharacterFromSprite('4', "Vanilla/Common/Status", 6)
                                     .AddCharacterFromSprite('5', "Vanilla/Common/Status", 7)
                                     .AddCharacterFromSprite('6', "Vanilla/Common/Status", 8)
                                     .AddCharacterFromSprite('7', "Vanilla/Common/Status", 9)
                                     .AddCharacterFromSprite('8', "Vanilla/Common/Status", 10)
                                     .AddCharacterFromSprite('9', "Vanilla/Common/Status", 11)
                                     .Build();


            var healthBarPixel = spriteMan.CreateAtlas()
                .SetName("StatusBarPixel")
                .SetTexture(statusBarTexture.Id)
                .AppendCoord(0, 0, 1, 1)
                .Build();

            var builder = worldMan.Create().SetName("GameHUD");

            builder.AddModule(renderableFactory.CreateRenderablePalette());
            builder.AddModule(renderableFactory.CreateRenderableBatch());


            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(IWorldBuilder builder)
        {
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<TextSystem>();
            builder.AddSystem<PaletteSystem>();
            builder.AddSystem<ScriptRunningSystem>();
        }

        private void BindHealth(IEntity statusBarEntity)
        {
            var player1Entity = entityMan.GetByTag("Players/P1").FirstOrDefault();

            if (player1Entity is null)
                return;

            var controlledEntityId = player1Entity.GetControlledEntityId();
            var controlledEntity = entityMan.GetById(controlledEntityId);

            triggerMan.OnDamagedEntity(controlledEntity, (s, a) =>
            {
                var spriteComponent = statusBarEntity.Get<SpriteComponent>();

                var percent = controlledEntity.Get<HealthComponent>().GetPercent();

                spriteComponent.Scale = new Vector2((int)(64 * percent), 1);
            });
        }

       
        private void Setup(IWorld world)
        {
            var hudCamera = cameraHelper.CreateCamera("Camera.GameHud", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                AddPalette(world);

                worldMan.RequestAddEntity(hudCamera, world.Id);

                var p1StatusBar = hudHelper.CreateHudElement("StatusBarP1", "Hud/StatusBar/P1", -160, 109);
                worldMan.RequestAddEntity(p1StatusBar, world.Id);

                var p1AmmoBar = hudHelper.CreateHudElement("AmmoBar", "Hud/AmmoBar/P1", 40, 115);
                worldMan.RequestAddEntity(p1AmmoBar, world.Id);

                var p1HealthBar = hudHelper.CreateHudElement("HealthBar", "Hud/HealthBar/P1", -128, 115);
                worldMan.RequestAddEntity(p1HealthBar, world.Id);

                BindHealth(p1HealthBar);

                var p1LivesCounter = hudHelper.CreateHudElement("LivesCounter", "Hud/LivesCounter/P1", -24, 120);
                worldMan.RequestAddEntity(p1LivesCounter, world.Id);

                var p1AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P1", 80, 120);
                worldMan.RequestAddEntity(p1AmmoCounter, world.Id);

                var p1KeysCounter = hudHelper.CreateHudElement("KeysCounter", "Hud/KeysCounter/P1", 128, 120);
                worldMan.RequestAddEntity(p1KeysCounter, world.Id);

                var p2StatusBar = hudHelper.CreateHudElement("StatusBarP2", "Hud/StatusBar/P2", -160, -120);
                worldMan.RequestAddEntity(p2StatusBar, world.Id);

                var p2AmmoBar = hudHelper.CreateHudElement("AmmoBar", "Hud/AmmoBar/P2", 40, -114);
                worldMan.RequestAddEntity(p2AmmoBar, world.Id);

                var p2HealthBar = hudHelper.CreateHudElement("HealthBar", "Hud/HealthBar/P2", -128, -114);
                worldMan.RequestAddEntity(p2HealthBar, world.Id);

                var p2LivesCounter = hudHelper.CreateHudElement("LivesCounter", "Hud/LivesCounter/P2", -24, -109);
                worldMan.RequestAddEntity(p2LivesCounter, world.Id);

                var p2AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P2", 80, -109);
                worldMan.RequestAddEntity(p2AmmoCounter, world.Id);

                var p2KeysCounter = hudHelper.CreateHudElement("KeysCounter", "Hud/KeysCounter/P2", 128, -109);
                worldMan.RequestAddEntity(p2KeysCounter, world.Id);

                var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
                hudViewport.SetViewportCamera(hudCamera.Id);

            }, singleTime: true);

        }

        #endregion Private Methods
    }
}