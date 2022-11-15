using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.LibOpenMpt;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Database.Xml.Extensions;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Model.Palettes;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Audio.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Scripting.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Control.Handlers;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OpenBreed.Sandbox
{
    internal class LuaEntityEventHandler<TEvent> : NLua.Method.LuaDelegate
    {
        void CallFunction(Entity entity, TEvent eventArgs)
        {
            object[] args = new object[] { entity, eventArgs };
            object[] inArgs = new object[] { entity, eventArgs };
            int[] outArgs = new int[] { };
            base.CallFunction(args, inArgs, outArgs);
        }
    }

    internal class LuaEventHandler<TEvent> : NLua.Method.LuaDelegate
    {
        void CallFunction(TEvent eventArgs)
        {
            object[] args = new object[] { eventArgs };
            object[] inArgs = new object[] { eventArgs };
            int[] outArgs = new int[] { };
            base.CallFunction(args, inArgs, outArgs);
        }
    }

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

        public ICore Create()
        {
            var appName = ProgramTools.AppProductName;
            var infoVersion = ProgramTools.AppInfoVerion;

            hostBuilder.SetupCoreManagers();
            hostBuilder.SetupDataGridFactory();

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

            hostBuilder.SetupLuaScripting((scriptMan, sp) =>
            {
                var eventsMan = sp.GetService<IEventsMan>();

                eventsMan.Subscribe<WorldInitializedEventArgs>(
                    (s,a) => scriptMan.TryInvokeFunction("WorldLoaded", a.WorldId));


                scriptMan.RegisterDelegateType(typeof(Action<Entity, WorldPausedEventArgs>), typeof(LuaEntityEventHandler<WorldPausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<Entity, WorldUnpausedEventArgs>), typeof(LuaEntityEventHandler<WorldUnpausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<Entity, AnimFinishedEventArgs>), typeof(LuaEntityEventHandler<AnimFinishedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<Entity, ClientResizedEventArgs>), typeof(LuaEntityEventHandler<ClientResizedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyDownEvent>), typeof(LuaEventHandler<KeyDownEvent>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyUpEvent>), typeof(LuaEventHandler<KeyUpEvent>));

                scriptMan.Expose("Entities", sp.GetService<IEntityMan>());
                scriptMan.Expose("Sounds", sp.GetService<ISoundMan>());
                scriptMan.Expose("Triggers", sp.GetService<ITriggerMan>());
                scriptMan.Expose("Logging", sp.GetService<ILogger>());
                scriptMan.Expose("Rendering", sp.GetService<IRenderingMan>());
                scriptMan.Expose("Stamps", sp.GetService<IStampMan>());
                scriptMan.Expose("Clips", sp.GetService<IClipMan<Entity>>());
                scriptMan.Expose("Shapes", sp.GetService<IShapeMan>());
                scriptMan.Expose("Items", sp.GetService<ItemsMan>());
                scriptMan.Expose("Texts", sp.GetService<TextsDataProvider>());
                scriptMan.Expose("Inputs", sp.GetService<IInputsMan>());

                var res = scriptMan.RunString(@"import('System')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs', 'OpenBreed.Wecs.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Components.Common', 'OpenBreed.Wecs.Components.Common.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Core', 'OpenBreed.Wecs.Systems.Core.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Control', 'OpenBreed.Wecs.Systems.Control.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Audio', 'OpenBreed.Wecs.Systems.Audio.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Rendering', 'OpenBreed.Wecs.Systems.Rendering.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Animation', 'OpenBreed.Wecs.Systems.Animation.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Physics', 'OpenBreed.Wecs.Systems.Physics.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Common', 'OpenBreed.Common.Extensions')");

                res = scriptMan.RunString(@"import('OpenBreed.Sandbox', 'OpenBreed.Sandbox.Extensions')");


                res = scriptMan.RunString(@"EntityTypes = {}");

                //var result = scriptMan.RunFile(@"D:\Projects\Programing\GIT\OpenBreed\OpenBreed.Common\src\OpenBreed.Database.Xml\Vanilla\Common\Scripts\Hud\FpsCounter.lua");


                //res = scriptMan.RunString(@"EntityTypes.FpsCounter.UpdateValue()");

            });

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
                p1.AddKeyBinding("Attacking", "Primary", Keys.RightControl);
                p1.AddKeyBinding("Walking", "Left", Keys.Left);
                p1.AddKeyBinding("Walking", "Right", Keys.Right);
                p1.AddKeyBinding("Walking", "Up", Keys.Up);
                p1.AddKeyBinding("Walking", "Down", Keys.Down);

                var p2 = playersMan.AddPlayer("P2");
                p2.RegisterInput(new DigitalJoyPlayerInput());
                p2.AddKeyBinding("Walking", "Left", Keys.A);
                p2.AddKeyBinding("Walking", "Right", Keys.D);
                p2.AddKeyBinding("Walking", "Up", Keys.W);
                p2.AddKeyBinding("Walking", "Down", Keys.S);
            });

            hostBuilder.SetupCollisionMan<Entity>((collisionMan, sp) =>
            {
                collisionMan.RegisterAbtaColliders();
            });

            hostBuilder.SetupBroadphaseFactory<Entity>();

            hostBuilder.SetupFixtureMan((s, a)=> { });

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
                systemFactory.SetupScriptingSystems(sp);
                systemFactory.SetupAudioSystems(sp);
                systemFactory.SetupPhysicsSystems(sp);
                systemFactory.SetupCoreSystems(sp);
                systemFactory.SetupControlSystems(sp);
                systemFactory.SetupAnimationSystems(sp);
                systemFactory.SetupPhysicsDebugSystem(sp);
                systemFactory.SetupGameSystems(sp);
            });

            XmlCommonComponents.Setup();
            XmlPhysicsComponents.Setup();
            XmlRenderingComponents.Setup();
            XmlAnimationComponents.Setup();
            XmlAudioComponents.Setup();
            XmlFsmComponents.Setup();
            XmlScriptingComponents.Setup();

            hostBuilder.SetupCommonComponentFactories();
            hostBuilder.SetupPhysicsComponentFactories();
            hostBuilder.SetupRenderingComponentFactories();
            hostBuilder.SetupAnimationComponentFactories();
            hostBuilder.SetupAudioComponentFactories();
            hostBuilder.SetupFsmComponentFactories();
            hostBuilder.SetupScriptingComponentFactories();

            hostBuilder.SetupComponentFactoryProvider((provider, sp) =>
            {
                provider.SetupCommonComponents(sp);
                provider.SetupPhysicsComponents(sp);
                provider.SetupRenderingComponents(sp);
                provider.SetupAnimationComponents(sp);
                provider.SetupAudioComponents(sp);
                provider.SetupFsmComponents(sp);
                provider.SetupScriptingComponents(sp);
            });

            hostBuilder.SetupEntityFactory((entityFactory, sp) =>
            {
            });

            hostBuilder.SetupWecsManagers();

            hostBuilder.SetupItemManager((itemsMap, sp) =>
            {
                itemsMap.RegisterAbtaItems();
            });

            hostBuilder.SetupFixtureTypes();
            hostBuilder.SetupViewportCreator();

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.SetupAnimationDataLoader<Entity>(sp);
                dataLoaderFactory.SetupMapLegacyDataLoader(sp);
                dataLoaderFactory.SetupTileSetDataLoader(sp);
                dataLoaderFactory.SetupTileStampDataLoader(sp);
                dataLoaderFactory.SetupSpriteSetDataLoader(sp);
                dataLoaderFactory.SetupPictureDataLoader(sp);
                dataLoaderFactory.SetupSoundSampleDataLoader(sp);
                dataLoaderFactory.SetupScriptDataLoader(sp);
            });

            hostBuilder.SetupFsmManager((fsmMan, sp) =>
            {
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
            hostBuilder.SetupGameSmartcardWorldHelper();
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
                var folderOptions = serviceProvider.GetService<IOptions<EnvironmentSettings>>();
                variableMan.RegisterVariable(typeof(string), folderOptions.Value.LegacyFolderPath, "Cfg.Options.ABTA.GameFolderPath");
            });

            hostBuilder.SetupXmlReadonlyDatabase();

            hostBuilder.ConfigureServices((sc) => sc.AddSingleton<FontHelper>());

            var host = hostBuilder.Build();

            return new Program(host, host.Services.GetService<IViewClient>());
        }

        #endregion Public Methods
    }

    public class Program : CoreBase
    {
        #region Private Fields

        private const string ABTA_PC_GAME_DB_FILE_NAME = "GameDatabase.ABTA.EPF.xml";

        private readonly IViewClient clientMan;
        private readonly LogConsolePrinter logConsolePrinter;

        #endregion Private Fields

        #region Public Constructors

        public Program(IHost host, IViewClient clientMan) :
            base(host)
        {
            this.clientMan = clientMan;

            var fsmMan = host.Services.GetService<IFsmMan>();
            fsmMan.SetupButtonStates(host.Services);
            fsmMan.SetupProjectileStates(host.Services);
            fsmMan.SetupActorAttackingStates(host.Services);
            fsmMan.SetupActorMovementStates(host.Services);
            fsmMan.CreateTurretRotationStates(host.Services);

            GetManager<IRenderingMan>();

            GetManager<IScriptMan>().Expose("Worlds", GetManager<IWorldMan>());


            clientMan.UpdateFrameEvent += (a) => OnUpdateFrame(a);
            clientMan.LoadEvent += () => OnLoad();

            logConsolePrinter = new LogConsolePrinter(GetManager<ILogger>());
            logConsolePrinter.StartPrinting();
        }

        #endregion Public Constructors

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
            GetManager<IScriptMan>().TryInvokeFunction("EngineInitialized");
        }

        #endregion Protected Methods

        #region Private Methods

        [STAThread]
        private static void Main(string[] args)
        {
            //            var amfFilePath = @"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\TITLE.AMF";
            //            var module = new OpenMpt.Module(amfFilePath);

            //            var bufferSize = 480;
            //            var buffer = new float[480];

            //            var frames = default(long);

            //            do
            //            {
            //                frames = module.ReadInterleavedStereo(48000, bufferSize, buffer);
            //            }
            //           while (frames != 0);
            //using (var file = File.OpenRead(amfFilePath))
            //{
            //    var amfReader = new Amf.AmfReader();
            //    var amf = amfReader.Read(file);
            //}

            //SetupCommandLine(args);


            var hostBuilder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();
            });

            hostBuilder.SetupCommandLine(args);

            var asm = Assembly.GetExecutingAssembly();

            var programFactory = new ProgramFactory(hostBuilder);

            var program = programFactory.Create();

            program.Run();
        }

        private void OnUpdateFrame(float dt)
        {
            GetManager<IPlayersMan>().ResetInputs();

            GetManager<IInputsMan>().Update();

            GetManager<IWorldMan>().Update(dt);

            GetManager<IJobsMan>().Update(dt);

            GetManager<ISoundMan>().Update();
        }

        private int ReadStream(InterleavedStereoModule module, int bufferSize, short[] buffer)
        {
            return 2 * module.Read(48000, bufferSize / 2, buffer);
        }

        private InterleavedStereoModule OpenMod(string moduleFilePath)
        {
            return new InterleavedStereoModule(moduleFilePath);
        }

        private void LoadGameWorld()
        {
            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var cameraHelper = GetManager<CameraHelper>();
            var entityMan = GetManager<IEntityMan>();
            var actorHelper = GetManager<ActorHelper>();
            var scriptMan = GetManager<IScriptMan>();
            var triggerMan = GetManager<ITriggerMan>();
            var worldGateHelper = GetManager<EntriesHelper>();
            var gameSettings = GetManager<IOptions<GameSettings>>();

            var mapLegacyLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();
            var mapTxtLoader = dataLoaderFactory.GetLoader<MapTxtDataLoader>();


            var levelName = gameSettings.Value.StartingLevelName;
            var gameWorld = mapLegacyLoader.Load(levelName);

            //var gameWorld = mapTxtLoader.Load(@"Content\Maps\demo_1.txt");

            //L1
            //var gameWorld = mapLegacyLoader.Load("Vanilla/1");
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
            var playerCamera = cameraHelper.CreateCamera("Camera.Player", 0, 0, 320, 240);

            playerCamera.Add(new PauseImmuneComponent());

            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
            gameViewport.SetViewportCamera(playerCamera.Id);

            //Follow John actor
            //var johnPlayerEntity = entityMan.GetByTag("John").First();
            var johnPlayerEntity = actorHelper.CreateDummyActor("John", new Vector2(0, 0));

            scriptMan.Expose("JohnPlayer", johnPlayerEntity);


            johnPlayerEntity.AddFollower(playerCamera);

            triggerMan.OnWorldInitialized(gameWorld, () =>
            {
                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, gameWorld.Name, 0);
            });
        }

        private void OnLoad()
        {
            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var pictureDataLoader = dataLoaderFactory.GetLoader<IPictureDataLoader>();

            var picture = pictureDataLoader.Load("Images.SMARTPIC.LBM");

            InitLua();

            GetManager<FixtureTypes>().Register();
            GetManager<FontHelper>().SetupGameFont();

            var spriteMan = GetManager<ISpriteMan>();
            var worldMan = GetManager<IWorldMan>();
            var scriptMan = GetManager<IScriptMan>();
            var tileMan = GetManager<ITileMan>();
            var textureMan = GetManager<ITextureMan>();
            var soundMan = GetManager<ISoundMan>();
            var worldGateHelper = GetManager<EntriesHelper>();
            var doorHelper = GetManager<DoorHelper>();
            var electicGateHelper = GetManager<ElectricGateHelper>();
            var pickableHelper = GetManager<PickableHelper>();
            var environmentHelper = GetManager<EnvironmentHelper>();
            var projectileHelper = GetManager<ProjectileHelper>();
            var actorHelper = GetManager<ActorHelper>();
            var teleportHelper = GetManager<TeleportHelper>();
            var cameraHelper = GetManager<CameraHelper>();
            var entityMan = GetManager<IEntityMan>();
            var triggerMan = GetManager<ITriggerMan>();
            var screenWorldHelper = GetManager<ScreenWorldHelper>();
            var gameHudWorldHelper = GetManager<GameHudWorldHelper>();
            var debugHudWorldHelper = GetManager<DebugHudWorldHelper>();
            var gameSmartcardWorldHelper = GetManager<GameSmartcardWorldHelper>();
            var renderingMan = GetManager<IRenderingMan>();

            //Create 4 sound sources, each one acting as a separate channel
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();


            //var amfFilePath = @"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\TITLE.AMF";
            //var mod = OpenMod(amfFilePath);
            //var musicId = soundMan.CreateStream("MUSIC", (bufferSize, buffer) => ReadStream(mod, bufferSize, buffer));
            //soundMan.PlayStream(musicId);

            var laserTex = textureMan.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            spriteMan.CreateAtlas()
                .SetTexture(laserTex.Id)
                .SetName("Atlases/Sprites/Projectiles/Laser")
                .AppendCoordsFromGrid(16, 16, 8, 1, 0, 0)
                .Build();

            actorHelper.RegisterCollisionPairs();
            worldGateHelper.RegisterCollisionPairs();
            projectileHelper.RegisterCollisionPairs();

            cameraHelper.CreateAnimations();
            projectileHelper.CreateAnimations();

            var screenWorld = screenWorldHelper.CreateWorld();

            renderingMan.Renderable = screenWorld.GetModule<IRenderableBatch>();

            debugHudWorldHelper.Create();

            LoadGameWorld();

            gameHudWorldHelper.Create();

            gameSmartcardWorldHelper.Create();


            //var hudWorld = worldMan.GetByName("GameHUD");

            //triggerMan.OnWorldInitialized(hudWorld, () =>
            //{
            //    var smartcardReaderCameraEntity = entityMan.GetByTag("Camera.SmartcardReader").First();

            //    var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            //    gameViewport.SetViewportCamera(smartcardReaderCameraEntity.Id);


            //}, singleTime: true);

            OnEngineInitialized();
        }

        private void InitLua()
        {
            //scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        #endregion Private Methods
    }
}