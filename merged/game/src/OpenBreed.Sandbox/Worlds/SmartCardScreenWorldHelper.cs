using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Texts;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class SmartcardScreenWorldHelper
    {
        #region Private Fields

        private readonly CameraHelper cameraHelper;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IPaletteMan paletteMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly VanillaStatusBarHelper hudHelper;
        private readonly IRenderableFactory renderableFactory;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        private readonly ISystemFactory systemFactory;
        private readonly ITriggerMan triggerMan;
        private readonly TextsDataProvider textsDataProvider;
        private readonly IViewClient viewClient;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public SmartcardScreenWorldHelper(ISystemFactory systemFactory,
                                  IRenderableFactory renderableFactory,
                                  IWorldMan worldMan,
                                  IFontMan fontMan,
                                  IViewClient viewClient,
                                  IEntityMan entityMan,
                                  IEntityFactory entityFactory,
                                  VanillaStatusBarHelper hudHelper,
                                  CameraHelper cameraHelper,
                                  PalettesDataProvider palettesDataProvider,
                                  IPaletteMan paletteMan,
                                  IRepositoryProvider repositoryProvider,
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
            this.palettesDataProvider = palettesDataProvider;
            this.paletteMan = paletteMan;
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.triggerMan = triggerMan;
            this.textsDataProvider = textsDataProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddBackground(IWorld world, int x, int y)
        {
            var timer = entityFactory.Create(@"ABTA\Templates\Common\SmartCardScreen\Background")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Background")
                .Build();

            worldMan.RequestAddEntity(timer, world.Id);
        }

        public void AddText(IWorld world, int x, int y)
        {
            var textEntity = entityFactory.Create(@"ABTA\Templates\Common\SmartCardScreen\Text")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Text")
                .Build();
            textEntity.SetText(0, string.Empty);
            worldMan.RequestAddEntity(textEntity, world.Id);
        }

        public void Create()
        {
            var builder = worldMan.Create().SetName(WorldNames.SMARTCARD_SCREEN);

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Vanilla/Common/SmartCardScreen/Palette");

            var paletteEntity = entityMan.Create(tag: $"Palettes/{WorldNames.SMARTCARD_SCREEN}");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray())
                .SetColors(Enumerable.Range(0, 64).Select(idx => PaletteHelper.ToColor4(MyColor.FromArgb(255, 0, 168, 168))).ToArray(), 32);

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

        private void Setup(IWorld world)
        {
            var smartCardCamera = cameraHelper.CreateCamera($"Camera.{WorldNames.SMARTCARD_SCREEN}", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                smartCardCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

                worldMan.RequestAddEntity(smartCardCamera, world.Id);
                AddBackground(world, -320, -272);
                AddText(world, - 320 / 2 + 20 , 240 / 2 - 38);
            }, singleTime: true);
        }

        #endregion Private Methods
    }
}