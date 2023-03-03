using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
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

        #endregion Private Fields

        #region Public Constructors

        public GameHudWorldHelper(ISystemFactory systemFactory, 
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
                                  SpriteAtlasDataProvider spriteAtlasDataProvider)
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
        }

        private void Setup(IWorld world)
        {
            var hudCamera = cameraHelper.CreateCamera("Camera.GameHud", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                AddPalette(world);

                worldMan.RequestAddEntity(hudCamera, world.Id);

                var p1StatusBar = hudHelper.CreateHudElement("StatusBarP1", "P1.StatusBar", -160, 109);
                worldMan.RequestAddEntity(p1StatusBar, world.Id);

                //var p1AmmoBar = hudHelper.CreateHudElement("AmmoBar", "P1.AmmoBar", 20, 112);
                //p1AmmoBar.EnterWorld(world.Id);

                //var p1HealthBar = hudHelper.CreateHudElement("HealthBar", "P1.HealthBar", -124, 112);
                //p1HealthBar.EnterWorld(world.Id);

                var p1LivesCounter = hudHelper.CreateHudElement("LivesCounter", "P1.LivesCounter", -24, 120);
                worldMan.RequestAddEntity(p1LivesCounter, world.Id);

                var p1AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "P1.AmmoCounter", 80, 120);
                worldMan.RequestAddEntity(p1AmmoCounter, world.Id);

                var p1KeysCounter = hudHelper.CreateHudElement("KeysCounter", "P1.KeysCounter", 128, 120);
                worldMan.RequestAddEntity(p1KeysCounter, world.Id);

                var p2StatusBar = hudHelper.CreateHudElement("StatusBarP2", "P2.StatusBar", -160, -120);
                worldMan.RequestAddEntity(p2StatusBar, world.Id);


                //var p2AmmoBar = hudHelper.CreateHudElement("AmmoBar", "P2.AmmoBar", 20, -117);
                //p2AmmoBar.EnterWorld(world.Id);

                //var p2HealthBar = hudHelper.CreateHudElement("HealthBar", "P2.HealthBar", -124, -117);
                //p2HealthBar.EnterWorld(world.Id);

                var p2LivesCounter = hudHelper.CreateHudElement("LivesCounter", "P2.LivesCounter", -24, -109);
                worldMan.RequestAddEntity(p2LivesCounter, world.Id);

                var p2AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "P2.AmmoCounter", 80, -109);
                worldMan.RequestAddEntity(p2AmmoCounter, world.Id);

                var p2KeysCounter = hudHelper.CreateHudElement("KeysCounter", "P2.KeysCounter", 128, -109);
                worldMan.RequestAddEntity(p2KeysCounter, world.Id);

                var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
                hudViewport.SetViewportCamera(hudCamera.Id);

            }, singleTime: true);

        }

        #endregion Private Methods
    }
}