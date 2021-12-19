using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Core;
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
            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
            builder.AddSystem(systemFactory.Create<TextSystem>());

        }

        private void Setup(World world)
        {
            var hudCamera = cameraHelper.CreateCamera(0, 0, 320, 240);

            hudCamera.Tag = "GameHudCamera";

            hudCamera.EnterWorld(world.Id);

            hudHelper.AddP1StatusBar(world);
            //hudHelper.AddAmmoBar(world, 20, 112);
            //hudHelper.AddHealthBar(world, -124, 112);
            hudHelper.AddLivesCounter(world, -24, 112);
            hudHelper.AddAmmoCounter(world, 80, 112);
            hudHelper.AddKeysCounter(world, 128, 112);

            hudHelper.AddP2StatusBar(world);
            //hudHelper.AddAmmoBar(world, 20, -117);
            //hudHelper.AddHealthBar(world, -124, -117);
            hudHelper.AddLivesCounter(world, -24, -117);
            hudHelper.AddAmmoCounter(world, 80, -117);
            hudHelper.AddKeysCounter(world, 128, -117);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            hudViewport.SetViewportCamera(hudCamera.Id);
        }

        #endregion Private Methods
    }
}