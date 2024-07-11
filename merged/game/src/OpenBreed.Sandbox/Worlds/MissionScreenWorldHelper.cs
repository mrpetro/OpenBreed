using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class MissionScreenWorldHelper
    {
        #region Private Fields

        private readonly CameraHelper cameraHelper;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly VanillaStatusBarHelper hudHelper;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IRenderableFactory renderableFactory;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        private readonly ISystemFactory systemFactory;
        private readonly TextsDataProvider textsDataProvider;
        private readonly ITriggerMan triggerMan;
        private readonly IViewClient viewClient;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public MissionScreenWorldHelper(ISystemFactory systemFactory,
                                  IRenderableFactory renderableFactory,
                                  IWorldMan worldMan,
                                  IFontMan fontMan,
                                  IViewClient viewClient,
                                  IEntityMan entityMan,
                                  IEntityFactory entityFactory,
                                  VanillaStatusBarHelper hudHelper,
                                  CameraHelper cameraHelper,
                                  IRepositoryProvider repositoryProvider,
                                  IPaletteMan paletteMan,
                                  PalettesDataProvider palettesDataProvider,
                                  IDataLoaderFactory dataLoaderFactory,
                                  SpriteAtlasDataProvider spriteAtlasDataProvider,
                                  ITriggerMan triggerMan,
                                  TextsDataProvider textsDataProvider)
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
            this.paletteMan = paletteMan;
            this.palettesDataProvider = palettesDataProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.triggerMan = triggerMan;
            this.textsDataProvider = textsDataProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Create()
        {
            var builder = worldMan.Create().SetName("MissionScreen");

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddBackground(IWorld world, int x, int y)
        {
            var timer = entityFactory.Create(@"ABTA\Templates\Common\MissionScreen\Background")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("MissionScreen/Background")
                .Build();

            worldMan.RequestAddEntity(timer, world.Id);
        }

        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Vanilla/Common/MissionScreen/Palette");

            var paletteEntity = entityMan.Create(tag: $"Palettes/{WorldNames.MISSION_SCREEN}");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray())
                .SetColors(Enumerable.Range(0, 64).Select(idx => Color4.White).ToArray(), 32);

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

            worldMan.RequestAddEntity(paletteEntity, world.Id);

            return palette.Id;
        }

        private void AddSystems(IWorldBuilder builder)
        {
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<PictureSystem>();
            builder.AddSystem<TextSystem>();
        }

        private void AddText(IWorld world, int x, int y, string text = "")
        {
            var textEntity = entityFactory.Create(@"ABTA\Templates\Common\MissionScreen\Text")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("MissionScreen/Text")
                .Build();
            textEntity.SetText(0, text);
            worldMan.RequestAddEntity(textEntity, world.Id);
        }

        private void Setup(IWorld world)
        {
            var missionScreenCamera = cameraHelper.CreateCamera("Camera.MissionScreen", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                missionScreenCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

                worldMan.RequestAddEntity(missionScreenCamera, world.Id);

                AddBackground(world, -320, -272);
                AddText(world, -320 / 2 + 48, 240 / 2 - 24);
            }, singleTime: true);
        }

        #endregion Private Methods
    }
}