using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Texts;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Worlds
{
    public class SmartcardScreenWorldHelper
    {
        #region Private Fields

        private readonly CameraHelper cameraHelper;
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
            var timer = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\SmartCardScreen\Background.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Background")
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddText(IWorld world, int x, int y)
        {
            var textEntity = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\SmartCardScreen\Text.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Text")
                .Build();
            textEntity.SetText(0, string.Empty);
            textEntity.EnterWorld(world.Id);
        }

        public void Create()
        {
            var builder = worldMan.Create().SetName("SmartCardScreen");

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
            builder.AddSystem<PictureSystem>();
            builder.AddSystem<TextSystem>();
        }

        private void Setup(IWorld world)
        {
            var smartCardCamera = cameraHelper.CreateCamera("Camera.SmartCardScreen", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                smartCardCamera.EnterWorld(world.Id);
                AddBackground(world, 0, 0);
                AddText(world, - 320 / 2 + 20 , 240 / 2 - 38);
            }, singleTime: true);
        }

        #endregion Private Methods
    }
}