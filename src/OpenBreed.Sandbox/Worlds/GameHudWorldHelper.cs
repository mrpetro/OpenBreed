using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
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
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
        }

        #endregion Public Constructors

        #region Public Methods


        private PaletteModel GetPaletteModel(string paletteName)
        {
            var paletteEntity = entityMan.GetByTag(paletteName).FirstOrDefault();

            if(paletteEntity is null)
                return PaletteModel.NullPalette;

            var paletteComponent = paletteEntity.TryGet<PaletteComponent>();

            if (paletteComponent is null)
                return PaletteModel.NullPalette;

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName(paletteName);
            for (int i = 0; i < paletteComponent.Colors.Length; i++)
            {
                var c = paletteComponent.Colors[i];

                paletteBuilder.SetColor(i, System.Drawing.Color.FromArgb(255, c.R, c.G, c.B));
            }

            return paletteBuilder.Build();
        }

        public void Create()
        {
            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbStatusBarSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById("Vanilla/Common/Status");

            var spriteSet = spriteAtlasDataProvider.GetSpriteSet(dbStatusBarSpriteAtlas.Id);

            var paletteModel = GetPaletteModel("GameWorld/Palette/CMAP");

            var colors = paletteModel.GetColors(0, 64);

            paletteModel.SetColors(64, colors);

            var spriteAtlas = loader.Load(dbStatusBarSpriteAtlas.Id, paletteModel);

            //Create FontAtlas
            var fontAtlas = fontMan.Create()
                                     .SetName("StatusBar")
                                     .AddCharacterFromSprite('0', "Vanilla/Common/Status#2", 0)
                                     .AddCharacterFromSprite('1', "Vanilla/Common/Status#3", 0)
                                     .AddCharacterFromSprite('2', "Vanilla/Common/Status#4", 0)
                                     .AddCharacterFromSprite('3', "Vanilla/Common/Status#5", 0)
                                     .AddCharacterFromSprite('4', "Vanilla/Common/Status#6", 0)
                                     .AddCharacterFromSprite('5', "Vanilla/Common/Status#7", 0)
                                     .AddCharacterFromSprite('6', "Vanilla/Common/Status#8", 0)
                                     .AddCharacterFromSprite('7', "Vanilla/Common/Status#9", 0)
                                     .AddCharacterFromSprite('8', "Vanilla/Common/Status#10", 0)
                                     .AddCharacterFromSprite('9', "Vanilla/Common/Status#11", 0)
                                     .Build();



            var builder = worldMan.Create().SetName("GameHUD");

            builder.AddModule(renderableFactory.CreateRenderableBatch());


            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(WorldBuilder builder)
        {
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<TextSystem>();

        }

        private void Setup(IWorld world)
        {
            var hudCamera = cameraHelper.CreateCamera("Camera.GameHud", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                hudCamera.EnterWorld(world.Id);

                var p1StatusBar = hudHelper.CreateHudElement("StatusBarP1", "P1.StatusBar", -160, 109);
                p1StatusBar.EnterWorld(world.Id);

                //var p1AmmoBar = hudHelper.CreateHudElement("AmmoBar", "P1.AmmoBar", 20, 112);
                //p1AmmoBar.EnterWorld(world.Id);

                //var p1HealthBar = hudHelper.CreateHudElement("HealthBar", "P1.HealthBar", -124, 112);
                //p1HealthBar.EnterWorld(world.Id);

                var p1LivesCounter = hudHelper.CreateHudElement("LivesCounter", "P1.LivesCounter", -24, 120);
                p1LivesCounter.EnterWorld(world.Id);

                var p1AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "P1.AmmoCounter", 80, 120);
                p1AmmoCounter.EnterWorld(world.Id);

                var p1KeysCounter = hudHelper.CreateHudElement("KeysCounter", "P1.KeysCounter", 128, 120);
                p1KeysCounter.EnterWorld(world.Id);

                var p2StatusBar = hudHelper.CreateHudElement("StatusBarP2", "P2.StatusBar", -160, -120);
                p2StatusBar.EnterWorld(world.Id);

                //var p2AmmoBar = hudHelper.CreateHudElement("AmmoBar", "P2.AmmoBar", 20, -117);
                //p2AmmoBar.EnterWorld(world.Id);

                //var p2HealthBar = hudHelper.CreateHudElement("HealthBar", "P2.HealthBar", -124, -117);
                //p2HealthBar.EnterWorld(world.Id);

                var p2LivesCounter = hudHelper.CreateHudElement("LivesCounter", "P2.LivesCounter", -24, -109);
                p2LivesCounter.EnterWorld(world.Id);

                var p2AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "P2.AmmoCounter", 80, -109);
                p2AmmoCounter.EnterWorld(world.Id);

                var p2KeysCounter = hudHelper.CreateHudElement("KeysCounter", "P2.KeysCounter", 128, -109);
                p2KeysCounter.EnterWorld(world.Id);

                var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
                hudViewport.SetViewportCamera(hudCamera.Id);

            }, singleTime: true);

        }

        #endregion Private Methods
    }
}