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
using OpenBreed.Wecs.Events;
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
    public class GameSmartcardWorldHelper
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
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public GameSmartcardWorldHelper(ISystemFactory systemFactory, 
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
                                  ITriggerMan triggerMan)
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
        }

        #endregion Public Constructors

        #region Public Methods

        public void Create()
        {
            var builder = worldMan.Create().SetName("SmartCardScreen");

            builder.AddModule(renderableFactory.CreateRenderableBatch());


            AddSystems(builder);

            Setup(builder.Build());
        }


        private void PerformShowSmartcardReader(params object[] args)
        {
            //Pause game world
            //Fade out player camera
            //Switch viewport to SmartcardWorld camera
            //Fade in SmartcardWorld camera
            //Start printing text at screen
            //Wait 

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
            var smartCardCamera = cameraHelper.CreateCamera("Camera.SmartcardReader", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                smartCardCamera.EnterWorld(world.Id);

                //var p2KeysCounter = hudHelper.CreateHudElement("KeysCounter", "P2.KeysCounter", 128, -117);
                //p2KeysCounter.EnterWorld(world.Id);

                //var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
                //hudViewport.SetViewportCamera(smartCardCamera.Id);

            }, singleTime: true);


        }

        #endregion Private Methods
    }
}