using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Database.Xml.Extensions;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Game;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Button;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;

using OpenBreed.Sandbox.Entities.Turret;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Control.Handlers;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OpenBreed.Core.Extensions;

namespace OpenBreed.Sandbox
{
    public class ProgramFactory
    {
        private readonly IHostBuilder hostBuilder;
        #region Public Constructors

        public ProgramFactory(IHostBuilder hostBuilder)
        {
            this.hostBuilder = hostBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public ICore Create(string gameDbFilePath, string gameFolderPath)
        {
            var appName = ProgramTools.AppProductName;
            var infoVersion = ProgramTools.AppInfoVerion;

            hostBuilder.SetupCoreManagers();

            hostBuilder.SetupViewClient(640, 480, $"{appName} v{infoVersion}");

            hostBuilder.SetupBuilderFactory((builderFactory, sp) =>
            {
                builderFactory.SetupPhysicsBuilderFactories(sp);
                builderFactory.SetupRenderingComponents(sp);
                builderFactory.SetupAnimationBuilderFactories(sp);
            });

            hostBuilder.SetupABFormats();
            hostBuilder.SetupModelProvider();
            hostBuilder.SetupDataProviders();

            hostBuilder.SetupFrameUpdaterMan<Entity>((frameUpdaterMan, sp) =>
            {
                new SpriteComponentAnimator(frameUpdaterMan, sp.GetService<ISpriteMan>());
            });

            hostBuilder.SetupClipMan<Entity>();

            hostBuilder.SetupLuaScripting();

            hostBuilder.SetupInputMan((inpitsMan, sp) =>
            {
                inpitsMan.RegisterHandler(new DigitalJoyInputHandler());
                inpitsMan.RegisterHandler(new ButtonInputHandler());
            });

            hostBuilder.SetupPlayersMan((playersMan, sp) =>
            {
                var p1 = playersMan.AddPlayer("P1");
                p1.RegisterInput(new ButtonPlayerInput());
                p1.RegisterInput(new DigitalJoyPlayerInput());
                p1.AddKeyBinding("Attacking", "Primary", Key.ControlRight);
                p1.AddKeyBinding("Walking", "Left", Key.Left);
                p1.AddKeyBinding("Walking", "Right", Key.Right);
                p1.AddKeyBinding("Walking", "Up", Key.Up);
                p1.AddKeyBinding("Walking", "Down", Key.Down);

                var p2 = playersMan.AddPlayer("P2");
                p2.RegisterInput(new DigitalJoyPlayerInput());
                p2.AddKeyBinding("Walking", "Left", Key.A);
                p2.AddKeyBinding("Walking", "Right", Key.D);
                p2.AddKeyBinding("Walking", "Up", Key.W);
                p2.AddKeyBinding("Walking", "Down", Key.S);
            });

            hostBuilder.SetupGenericPhysicsManagers<Entity>();

            hostBuilder.SetupShapeMan((shapeMan, sp) =>
            {
                shapeMan.Register("Shapes/Point_14_14", new PointShape(14, 14));
                shapeMan.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
                shapeMan.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
                shapeMan.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
                shapeMan.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
                shapeMan.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
                shapeMan.Register("Shapes/Box_0_0_28_28", new BoxShape(0, 0, 28, 28));
            });

            hostBuilder.SetupOpenALManagers();
            hostBuilder.SetupOpenGLManagers();

            hostBuilder.SetupSystemFactory((systemFactory, sp) =>
            {
                systemFactory.SetupRenderingSystems(sp);
                systemFactory.SetupAudioSystems(sp);
                systemFactory.SetupPhysicsSystems(sp);
                systemFactory.SetupCoreSystems(sp);
                systemFactory.SetupControlSystems(sp);
                systemFactory.SetupAnimationSystems(sp);
                systemFactory.SetupPhysicsDebugSystem(sp);
                systemFactory.SetupUnknownMapCellDisplaySystem(sp);
                systemFactory.SetupGroupMapCellDisplaySystem(sp);
            });

            XmlCommonComponents.Setup();
            XmlPhysicsComponents.Setup();
            XmlRenderingComponents.Setup();
            XmlAnimationComponents.Setup();
            XmlFsmComponents.Setup();

            hostBuilder.SetupCommonComponentFactories();
            hostBuilder.SetupPhysicsComponentFactories();
            hostBuilder.SetupRenderingComponentFactories();
            hostBuilder.SetupAnimationComponentFactories();
            hostBuilder.SetupFsmComponentFactories();

            hostBuilder.SetupEntityFactory((entityFactory, sp) =>
            {
                entityFactory.SetupCommonComponents(sp);
                entityFactory.SetupPhysicsComponents(sp);
                entityFactory.SetupRenderingComponents(sp);
                entityFactory.SetupAnimationComponents(sp);
                entityFactory.SetupFsmComponents(sp);
            });

            hostBuilder.SetupWecsManagers();
            hostBuilder.SetupItemManager();
            hostBuilder.SetupFixtureTypes();
            hostBuilder.SetupViewportCreator();

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.SetupAnimationDataLoader<Entity>(sp);
                dataLoaderFactory.SetupMapLegacyDataLoader(sp);
                dataLoaderFactory.SetupTileSetDataLoader(sp);
                dataLoaderFactory.SetupTileStampDataLoader(sp);
                dataLoaderFactory.SetupSpriteSetDataLoader(sp);
                dataLoaderFactory.SetupSoundSampleDataLoader(sp);
            });

            hostBuilder.SetupFsmManager((fsmMan, sp) => {
                //fsmMan.SetupButtonStates(sp);
                //fsmMan.SetupProjectileStates(sp);
                //fsmMan.SetupDoorStates(sp);
                //fsmMan.SetupPickableStates(sp);
                //fsmMan.SetupActorAttackingStates(sp);
                //fsmMan.SetupActorMovementStates(sp);
                //fsmMan.CreateTurretRotationStates(sp);
            });

            hostBuilder.SetupScreenWorldHelper();
            hostBuilder.SetupGameHudWorldHelper();
            hostBuilder.SetupDebugHudWorldHelper();
            hostBuilder.SetupEntriesHelper();
            hostBuilder.SetupDoorHelper();
            hostBuilder.SetupHudHelper();
            hostBuilder.SetupVanillaStatusBarHelper();
            hostBuilder.SetupElectricGateHelper();
            hostBuilder.SetupPickableHelper();
            hostBuilder.SetupGenericCellHelper();
            hostBuilder.SetupEnvironmentHelper();
            hostBuilder.SetupCameraHelper();
            hostBuilder.SetupTeleportHelper();
            hostBuilder.SetupProjectileHelper();
            hostBuilder.SetupActorHelper();
            hostBuilder.SetupDynamicResolver();

            hostBuilder.SetupVariableManager((variableMan, serviceProvider) =>
            {
                variableMan.RegisterVariable(typeof(string), gameFolderPath, "Cfg.Options.ABTA.GameFolderPath");
            });

            hostBuilder.SetupXmlReadonlyDatabase(gameDbFilePath);

            var host = hostBuilder.Build();

            return new Program(host, host.Services.GetService<IViewClient>());
        }

        #endregion Public Methods
    }

    public class Program : CoreBase
    {
        #region Private Fields

        private const string ABTA_PC_GAME_DB_FILE_NAME = "GameDatabase.ABTA.EPF.xml";

        private readonly IScriptMan scriptMan;
        private readonly IViewClient clientMan;
        private readonly IRenderingMan renderingMan;
        private readonly LogConsolePrinter logConsolePrinter;

        #endregion Private Fields

        #region Public Constructors

        public Program(IHost host, IViewClient clientMan) :
            base(host)
        {
            this.clientMan = clientMan;

            scriptMan = GetManager<IScriptMan>();
            renderingMan = GetManager<IRenderingMan>();

            GetManager<ICollisionMan<Entity>>().RegisterAbtaColliders();
            GetManager<ItemsMan>().RegisterAbtaItems();

            var fsmMan = host.Services.GetService<IFsmMan>();
            fsmMan.SetupButtonStates(host.Services);
            fsmMan.SetupProjectileStates(host.Services);
            fsmMan.SetupDoorStates(host.Services);
            fsmMan.SetupPickableStates(host.Services);
            fsmMan.SetupActorAttackingStates(host.Services);
            fsmMan.SetupActorMovementStates(host.Services);
            fsmMan.CreateTurretRotationStates(host.Services);

            clientMan.UpdateFrameEvent += (s, a) => OnUpdateFrame(a);
            clientMan.LoadEvent += (s, a) => OnLoad();

            logConsolePrinter = new LogConsolePrinter(GetManager<ILogger>());
            logConsolePrinter.StartPrinting();
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        public override void Run()
        {
            clientMan.Run();
        }

        public override void Exit()
        {
            clientMan.Exit();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnEngineInitialized()
        {
            scriptMan.TryInvokeFunction("EngineInitialized");
        }

        #endregion Protected Methods

        #region Private Methods

        [STAThread]
        private static void Main(string[] args)
        {

            //RunWithHostBuilder(args).Wait();


            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments.");
                Console.WriteLine($"Usage:");
                Console.WriteLine($"{System.AppDomain.CurrentDomain.FriendlyName} <database XML file path> <vanilla game folder path>");
                return;
            }
            var asm = Assembly.GetExecutingAssembly();
            var gameDbFileName = args[0];
            var vanillaGameFolderPath = args[1];
            var execFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var gameDbFilePath = Path.Combine(execFolderPath, gameDbFileName);

            var programFactory = new ProgramFactory(new HostBuilder());

            var program = programFactory.Create(gameDbFilePath, vanillaGameFolderPath);

            //program.Sounds.Sounds.PlaySound(0);

            program.Run();
        }

        private static async Task RunWithHostBuilder(string[] args)
        {
            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: true);
                  config.AddEnvironmentVariables();

                  if (args != null)
                  {
                      config.AddCommandLine(args);
                  }
              });
              //.ConfigureServices((hostContext, services) =>
              //{
              //    services.AddOptions();
              //    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
//
              //    services.AddSingleton<IHostedService, PrintTextToConsoleService>();
              //});
              //.ConfigureLogging((hostingContext, logging) => {
              //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
              //    logging.AddConsole();
              //});

            await builder.RunConsoleAsync();
        }

        private void OnUpdateFrame(float dt)
        {
            GetManager<IEventQueue>().Fire();

            GetManager<IPlayersMan>().ResetInputs();

            GetManager<IInputsMan>().Update();

            GetManager<IWorldMan>().Update(dt);

            GetManager<IJobsMan>().Update(dt);
        }

        private void OnLoad()
        {
            InitLua();

            GetManager<FixtureTypes>().Register();

            var spriteMan = GetManager<ISpriteMan>();
            var tileMan = GetManager<ITileMan>();
            var textureMan = GetManager<ITextureMan>();
            var soundMan = GetManager<ISoundMan>();

            //Create 4 sound sources, each one acting as a separate channel
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();

            var laserTex = textureMan.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            spriteMan.CreateAtlas()
                .SetTexture(laserTex.Id)
                .SetName("Atlases/Sprites/Projectiles/Laser")
                .AppendCoordsFromGrid(16, 16, 8, 1, 0, 0)
                .Build();

            var worldGateHelper = GetManager<EntriesHelper>();
            var doorHelper = GetManager<DoorHelper>();
            var electicGateHelper = GetManager<ElectricGateHelper>();
            var pickableHelper = GetManager<PickableHelper>();
            var environmentHelper = GetManager<EnvironmentHelper>();
            var projectileHelper = GetManager<ProjectileHelper>();
            var actorHelper = GetManager<ActorHelper>();
            var teleportHelper = GetManager<TeleportHelper>();
            var cameraHelper = GetManager<CameraHelper>();

            actorHelper.RegisterCollisionPairs();
            worldGateHelper.RegisterCollisionPairs();
            teleportHelper.RegisterCollisionPairs();
            projectileHelper.RegisterCollisionPairs();

            cameraHelper.CreateAnimations();
            projectileHelper.CreateAnimations();

            //manCollection.SetupButtonStates();
            //manCollection.SetupProjectileStates();
            //manCollection.SetupDoorStates();
            //manCollection.SetupPickableStates();
            //manCollection.SetupActorAttackingStates();
            //manCollection.SetupActorMovementStates();
            //manCollection.CreateTurretRotationStates();

            var screenWorldHelper = GetManager<ScreenWorldHelper>();

            var screenWorld = screenWorldHelper.CreateWorld();

            renderingMan.Renderable = screenWorld.GetModule<IRenderableBatch>();
            //renderingMan.ScreenWorld = screenWorldHelper.CreateWorld();

            var debugHudWorldHelper = GetManager<DebugHudWorldHelper>();
            debugHudWorldHelper.Create();

            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var mapLegacyLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();
            var mapTxtLoader = dataLoaderFactory.GetLoader<MapTxtDataLoader>();

            var entityMan = GetManager<IEntityMan>();

            //var gameWorld = mapTxtLoader.Load(@"Content\Maps\demo_1.txt");

            //L1
            var gameWorld = mapLegacyLoader.Load("Vanilla/1");
            //LD
            //var gameWorld = mapLegacyLoader.Load("Vanilla/7");
            //L3
            //var gameWorld = mapLegacyLoader.Load("Vanilla/28");
            //L4
            //var gameWorld = mapLegacyLoader.Load("Vanilla/2");
            //L5
            //var gameWorld = mapLegacyLoader.Load("Vanilla/16");
            //L6
            //var gameWorld = mapLegacyLoader.Load("Vanilla/21");
            //L7
            //var gameWorld = mapLegacyLoader.Load("Vanilla/12");
            //L8
            //var gameWorld = mapLegacyLoader.Load("Vanilla/47");

            //var playerCamera = cameraHelper.CreateCamera(0, 0, 640, 480);
            var playerCamera = cameraHelper.CreateCamera(0, 0, 320, 240);

            playerCamera.Add(new PauseImmuneComponent());
            playerCamera.Tag = "PlayerCamera";

            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
            gameViewport.SetViewportCamera(playerCamera.Id);

            //Follow John actor
            //var johnPlayerEntity = entityMan.GetByTag("John").First();
            var johnPlayerEntity = actorHelper.CreatePlayerActor(new Vector2(0, 0));
            johnPlayerEntity.Tag = "John";

            johnPlayerEntity.AddFollower(playerCamera);

            GetManager<IEventsMan>().SubscribeEx<WorldInitializedEventArgs>((s, a) =>
            {
                if (a.WorldId != gameWorld.Id)
                    return;

                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, gameWorld.Id, 0);
            });


            var gameHudWorldHelper = GetManager<GameHudWorldHelper>();
            gameHudWorldHelper.Create();

            OnEngineInitialized();
        }

        private void InitLua()
        {
            //scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        #endregion Private Methods
    }
}