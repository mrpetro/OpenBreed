using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Rendering.Interface.Data;
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
        private readonly IWorldMan worldMan;
        private readonly IViewClient viewClient;
        private readonly IEntityMan entityMan;
        private readonly IEntityFactory entityFactory;
        private readonly HudHelper hudHelper;
        private readonly CameraHelper cameraHelper;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;

        #endregion Private Fields

        #region Public Constructors

        public GameHudWorldHelper(ISystemFactory systemFactory, IWorldMan worldMan, IViewClient viewClient, IEntityMan entityMan, IEntityFactory entityFactory, HudHelper hudHelper, CameraHelper cameraHelper, IRepositoryProvider repositoryProvider, IDataLoaderFactory dataLoaderFactory )
        {
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.viewClient = viewClient;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.hudHelper = hudHelper;
            this.cameraHelper = cameraHelper;
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
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

            var paletteModel = GetPaletteModel("GameWorld/Palette/CMAP");

            loader.Load(dbStatusBarSpriteAtlas.Id, paletteModel);

            var builder = worldMan.Create().SetName("GameHUD");

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(WorldBuilder builder)
        {
            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<SpriteSystem>());
        }

        private void Setup(World world)
        {
            var hudCamera = cameraHelper.CreateCamera(0, 0, 320, 240);

            hudCamera.Tag = "GameHudCamera";

            hudCamera.EnterWorld(world.Id);

            hudHelper.AddP1StatusBar(world);
            hudHelper.AddP2StatusBar(world);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            hudViewport.SetViewportCamera(hudCamera.Id);
        }

        #endregion Private Methods
    }
}