using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
using OpenBreed.Common.Tools;
using OpenBreed.Common.Windows.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Xml;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Input.Interface.Events;
using OpenBreed.Model;
using OpenBreed.Model.Extensions;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Audio.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Gui.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Scripting.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OpenBreed.Rendering.Interface.Extensions;
using OpenBreed.Core.Interface;
using OpenBreed.Core.Interface.Managers;

namespace OpenBreed.Sandbox
{
    internal class LuaEntityEventHandler<TEvent> : NLua.Method.LuaDelegate
    {
        void CallFunction(IEntity entity, TEvent eventArgs)
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

            hostBuilder.SetupDataHandlers();

            hostBuilder.SetupCoreManagers();
            hostBuilder.SetupDataGridFactory();

            hostBuilder.SetupGameWindow(640, 480, $"{appName} v{infoVersion}");
            hostBuilder.SetupGLWindow();
            hostBuilder.SetupGLRenderContextComponents();
            hostBuilder.SetupWindowsDrawingContext();

            hostBuilder.SetupBuilderFactory((builderFactory, sp) =>
            {
                builderFactory.SetupPhysicsBuilders(sp);
                builderFactory.SetupRenderingBuilders(sp);
                builderFactory.SetupAnimationBuilders(sp);
                builderFactory.SetupSandboxBuilders(sp);
            });

            hostBuilder.SetupModelProvider();
            hostBuilder.SetupDataProviders();

            hostBuilder.SetupFrameUpdaterMan<IEntity>((frameUpdaterMan, sp) =>
            {
                new SpriteComponentAnimator(frameUpdaterMan, sp.GetService<ISpriteMan>());
            });

            hostBuilder.SetupClipMan<IEntity>();

            hostBuilder.ConfigureServices(sc =>
            {
                sc.AddSingleton<CoordsTransformer>();
            });


            hostBuilder.SetupLuaScripting((scriptMan, sp) =>
            {
                var eventsMan = sp.GetService<IEventsMan>();

                eventsMan.Subscribe<WorldInitializedEventArgs>(
                    (a) => scriptMan.TryInvokeFunction("WorldLoaded", a.WorldId));


                scriptMan.RegisterDelegateType(typeof(Action<IEntity, WorldPausedEventArgs>), typeof(LuaEntityEventHandler<WorldPausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<IEntity, WorldUnpausedEventArgs>), typeof(LuaEntityEventHandler<WorldUnpausedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<IEntity, AnimFinishedEvent>), typeof(LuaEntityEventHandler<AnimFinishedEvent>));
                //scriptMan.RegisterDelegateType(typeof(Action<IEntity, ClientResizedEventArgs>), typeof(LuaEntityEventHandler<ClientResizedEventArgs>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyDownEvent>), typeof(LuaEventHandler<KeyDownEvent>));
                scriptMan.RegisterDelegateType(typeof(Action<KeyUpEvent>), typeof(LuaEventHandler<KeyUpEvent>));

                scriptMan.Expose("Entities", sp.GetService<IEntityMan>());
                scriptMan.Expose("Sounds", sp.GetService<ISoundMan>());
                scriptMan.Expose("Triggers", sp.GetService<ITriggerMan>());
                scriptMan.Expose("Logging", sp.GetService<ILogger>());
                scriptMan.Expose("Rendering", sp.GetService<IRenderingMan>());
                scriptMan.Expose("Stamps", sp.GetService<IStampMan>());
                scriptMan.Expose("Clips", sp.GetService<IClipMan<IEntity>>());
                scriptMan.Expose("Shapes", sp.GetService<IShapeMan>());
                scriptMan.Expose("Items", sp.GetService<ItemsMan>());
                scriptMan.Expose("Texts", sp.GetService<TextsDataProvider>());
                scriptMan.Expose("Inputs", sp.GetService<IInputsMan>());
                scriptMan.Expose("Worlds", sp.GetService<IWorldMan>());
                scriptMan.Expose("Coords", sp.GetService<CoordsTransformer>());

                var res = scriptMan.RunString(@"import('System')");
                res = scriptMan.RunString(@"import('OpenTK.Mathematics')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs', 'OpenBreed.Wecs.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Components.Common', 'OpenBreed.Wecs.Components.Common.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Core', 'OpenBreed.Wecs.Systems.Core.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Control', 'OpenBreed.Wecs.Systems.Control.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Control', 'OpenBreed.Wecs.Systems.Control')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Audio', 'OpenBreed.Wecs.Systems.Audio.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Rendering', 'OpenBreed.Wecs.Systems.Rendering.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Animation', 'OpenBreed.Wecs.Systems.Animation.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Physics', 'OpenBreed.Wecs.Systems.Physics.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Scripting', 'OpenBreed.Wecs.Systems.Scripting.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Wecs.Systems.Gui', 'OpenBreed.Wecs.Systems.Gui.Extensions')");
                res = scriptMan.RunString(@"import('OpenBreed.Common', 'OpenBreed.Common.Extensions')");

                res = scriptMan.RunString(@"import('OpenBreed.Sandbox', 'OpenBreed.Sandbox.Extensions')");
                res = scriptMan.RunString(@"import(' OpenBreed.Sandbox.Entities', ' OpenBreed.Sandbox.Entities')");
                res = scriptMan.RunString(@"import('OpenBreed.Sandbox', 'OpenBreed.Sandbox')");

                res = scriptMan.RunString(@"EntityTypes = {}");

                //var result = scriptMan.RunFile(@"D:\Projects\Programing\GIT\OpenBreed\OpenBreed.Common\src\OpenBreed.Database.Xml\Vanilla\Common\Scripts\Hud\FpsCounter.lua");


                //res = scriptMan.RunString(@"EntityTypes.FpsCounter.UpdateValue()");

            });

            hostBuilder.SetupInputMan((inpitsMan, sp) =>
            {
            });

            hostBuilder.SetupDefaultActionCodeProvider((codeProvider, sp) =>
            {
                codeProvider.Register(PlayerActions.Fire);
            });

            hostBuilder.SetupDefaultActionTriggerBinder((keyBinder, sp) =>
            {
                keyBinder.Bind(PlayerActions.MoveLeft, Keys.Left);
                keyBinder.Bind(PlayerActions.MoveRight, Keys.Right);
                keyBinder.Bind(PlayerActions.MoveDown, Keys.Down);
                keyBinder.Bind(PlayerActions.MoveUp, Keys.Up);
                keyBinder.Bind(PlayerActions.Fire, Keys.RightControl);
            });

            hostBuilder.SetupCollisionChecker();

            hostBuilder.SetupCollisionMan<IEntity>((collisionMan, sp) =>
            {
                collisionMan.RegisterAbtaColliders();
            });

            hostBuilder.SetupBroadphaseFactory<IEntity>();

            hostBuilder.SetupFixtureMan((s, a)=> { });
            hostBuilder.SetupModelTools();

            hostBuilder.SetupShapeMan((shapeMan, sp) =>
            {
                shapeMan.Register("Shapes/Point_14_14", new PointShape(14, 14));
                shapeMan.Register("Shapes/Point_0_0", new PointShape(0, 0));
                shapeMan.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
                shapeMan.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
                shapeMan.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
                shapeMan.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
                shapeMan.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
                shapeMan.Register("Shapes/Box_-24_-24_48_48", new BoxShape(-24, -24, 48, 48));
                shapeMan.Register("Shapes/Box_0_0_28_28", new BoxShape(0, 0, 28, 28));
                shapeMan.Register("Shapes/Box_-14_-14_28_28", new BoxShape(-14, -14, 28, 28));
                shapeMan.Register("Shapes/Circle_0_0_240", new CircleShape(new Vector2(0, 0), 240));
                shapeMan.Register("Shapes/Circle_0_0_120", new CircleShape(new Vector2(0, 0), 120));
                shapeMan.Register("Shapes/Circle_0_0_480", new CircleShape(new Vector2(0, 0), 480));
                shapeMan.Register("Shapes/Circle_0_0_40", new CircleShape(new Vector2(0, 0), 40));
                shapeMan.Register("Shapes/Circle_0_0_320", new CircleShape(new Vector2(0, 0), 320));
                shapeMan.Register("Shapes/Circle_0_0_160", new CircleShape(new Vector2(0, 0), 160));
            });

            hostBuilder.SetupOpenALManagers();
            hostBuilder.SetupOpenGLManagers();

            hostBuilder.SetupDefaultTypeAttributesProvider();
            hostBuilder.SetupDefaultSystemRequirementsProvider();
            hostBuilder.SetupDefaultEntityToSystemMatcher();


            hostBuilder.SetupCollisionVisualizingOptions();

            hostBuilder.SetupSystemFactory((systemFactory, sp) =>
            {
                systemFactory.SetupRenderingSystems(sp);
                systemFactory.SetupScriptingSystems(sp);
                systemFactory.SetupAudioSystems(sp);
                systemFactory.SetupPhysicsSystems(sp);
                systemFactory.SetupCoreSystems(sp);
                systemFactory.SetupControlSystems(sp);
                systemFactory.SetupAnimationSystems(sp);
                systemFactory.SetupGuiSystems(sp);
                systemFactory.SetupGameSystems(sp);
            });

            hostBuilder.SetupCommonComponents();
            hostBuilder.SetupPhysicsComponents();
            hostBuilder.SetupRenderingComponents();
            hostBuilder.SetupAnimationComponents();
            hostBuilder.SetupAudioComponents();
            hostBuilder.SetupFsmComponents();
            hostBuilder.SetupScriptingComponents();
            hostBuilder.SetupGuiComponents();
            hostBuilder.SetupSandboxComponents();

            hostBuilder.SetupComponentFactoryProvider();

            hostBuilder.SetupXmlEntityTemplateLoader();

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

            hostBuilder.ConfigureGraphicsDataLoaders();

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.RegisterGraphicsDataLoader(sp);
                dataLoaderFactory.SetupAnimationDataLoader<IEntity>(sp);
                dataLoaderFactory.SetupMapLegacyDataLoader(sp);
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
            hostBuilder.SetupMissionScreenWorldHelper();
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
            hostBuilder.SetupActorHelper();
            hostBuilder.SetupDynamicResolver();

            hostBuilder.SetupVariableManager((variableMan, serviceProvider) =>
            {
                var folderOptions = serviceProvider.GetService<IOptions<EnvironmentSettings>>();
                variableMan.RegisterVariable(typeof(string), folderOptions.Value.LegacyFolderPath, "Cfg.Options.ABTA.GameFolderPath");
            });

            hostBuilder.SetupXmlReadonlyDatabase();

            hostBuilder.ConfigureServices((sc) => sc.AddSingleton<FontHelper>());
            hostBuilder.ConfigureLogConsolePrinter();

            var host = hostBuilder.Build();

            return new Program(host);
        }

        #endregion Public Methods
    }

    public class Program : CoreBase
    {
        #region Private Fields

        private const string ABTA_PC_GAME_DB_FILE_NAME = "GameDatabase.ABTA.EPF.xml";

        private readonly IWindow window;
        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public Program(IHost host) :
            base(host)
        {
            host.RunAsync();

            window = host.Services.GetService<IWindow>();
            worldMan = host.Services.GetService<IWorldMan>();
            eventsMan = host.Services.GetService<IEventsMan>();

            eventsMan.Subscribe<WindowUpdateEvent>((a) => OnUpdateFrame(a.Dt));
            eventsMan.Subscribe<WindowLoadEvent>(OnWindowLoad);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            window.Run();
        }

        public override void Exit()
        {
            window.Exit();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnEngineInitialized()
        {
            GetManager<IScriptMan>().TryInvokeFunction("EngineInitialized");
        }

        #endregion Protected Methods

        #region Private Methods

        private void LuaConsoleInput(
            IScriptMan scriptMan,
            CollisionVisualizingOptions visualizingOptions)
        {

            do
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.Write("Command: ");
                var commandLine = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);


                if (!commandLine.Any())
                {
                    continue;

                }

                var command = commandLine.First();

                var exit = false;

                switch (command.ToLower())
                {
                    case "exit":
                        exit = true;
                        break;
                    case "collisions":
                        var options = commandLine.Skip(1).Take(1);

                        if (!options.Any())
                        {
                            Console.WriteLine("Missing option to 'collisions' command.");
                            continue;
                        }

                        var option = options.First().ToLower();

                        switch (option)
                        {
                            case "show":
                                visualizingOptions.Enabled = true;
                                continue;
                            case "hide":
                                visualizingOptions.Enabled = false;
                                continue;
                            default:
                                Console.WriteLine($"Invalid option ('{option}'). Accepted options: show, hide");
                                continue;
                        }
                    default:
                        break;
                }

                if (exit)
                {
                    break;
                };

                try
                {
                    scriptMan.RunString(command);
                }

                catch (NLua.Exceptions.LuaException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true);

            Exit();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            //var spriteMerger = new SpriteMarger();
            //var spriteSetBuilder = new SpriteSetBuilder();
            //var sprReader = new SPRReader(spriteSetBuilder);

            //var inputFileName = @"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\COMPFONT.SPR";

            //var fileStream = File.OpenRead(inputFileName);
            //var model = sprReader.Read(fileStream);
            //byte[] outData = default;
            //int width = -1;
            //int height = -1;
            //List<(int X, int Y, int Width, int Height)> bounds = default;
            //spriteMerger.Merge(model.Sprites, out outData, out width, out height, out bounds);

            //var bitmap = BitmapHelper.FromBytes(width, height, outData);

            //var outputFileName = Path.Combine(Path.GetDirectoryName(inputFileName), $"{Path.GetFileNameWithoutExtension(inputFileName)}.png");

            //bitmap.Save(outputFileName);

            //return;









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

            hostBuilder.SetupDefaultLogger();
            hostBuilder.SetupCommandLine(args);

            var asm = Assembly.GetExecutingAssembly();

            var programFactory = new ProgramFactory(hostBuilder);

            var program = programFactory.Create();

            program.Run();
        }

        private void OnUpdateFrame(float dt)
        {
            GetManager<IRenderingMan>().Update(dt);

            dt = Math.Min(1.0f/30.0f, dt);

            GetManager<IInputsMan>().Update();

            GetManager<IWorldMan>().Update(dt);

            GetManager<IJobsMan>().Update(dt);

            GetManager<ISoundMan>().Update();

            GetManager<IEntityMan>().Cleanup();
        }

        private int ReadStream(InterleavedStereoModule module, int bufferSize, short[] buffer)
        {
            return 2 * module.Read(48000, bufferSize / 2, buffer);
        }

        private InterleavedStereoModule OpenMod(string moduleFilePath)
        {
            return new InterleavedStereoModule(moduleFilePath);
        }

        private void InitPlayers()
        {

        }

        private void InitGameWorld()
        {
            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var cameraHelper = GetManager<CameraHelper>();
            var entityMan = GetManager<IEntityMan>();
            var actorHelper = GetManager<ActorHelper>();
            var scriptMan = GetManager<IScriptMan>();
            var triggerMan = GetManager<ITriggerMan>();
            var worldGateHelper = GetManager<EntriesHelper>();
            var gameSettings = GetManager<IOptions<GameSettings>>();
            var hudHelper = GetManager<HudHelper>();

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

            var player1Entity = entityMan.GetByTag("Players/P1").First();

            var johnPlayerEntity = actorHelper.CreatePlayerActor("John", new Vector2(0, 0));

            player1Entity.SetControlledEntity(johnPlayerEntity.Id);

            scriptMan.Expose("JohnPlayer", johnPlayerEntity);

            //hudHelper.AddCursor(gameWorld);

            johnPlayerEntity.AddFollower(playerCamera);

            triggerMan.OnEntityFollow(johnPlayerEntity, (s, a) =>
            {
                var followerEntity = entityMan.GetById(a.FollowerId);
                Glue(johnPlayerEntity, followerEntity);
            });

            triggerMan.OnWorldInitialized(gameWorld, () =>
            {
                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, playerCamera, gameWorld.Name, 0);
            });
        }

        private void Glue(IEntity followed, IEntity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();

            followerPos.Value = followedPos.Value;
        }

        private void LoadSandboxWorld(int width, int height)
        {
            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var cameraHelper = GetManager<CameraHelper>();
            var entityMan = GetManager<IEntityMan>();
            var actorHelper = GetManager<ActorHelper>();
            var scriptMan = GetManager<IScriptMan>();
            var triggerMan = GetManager<ITriggerMan>();
            var worldMan = GetManager<IWorldMan>();
            var worldGateHelper = GetManager<EntriesHelper>();
            var gameSettings = GetManager<IOptions<GameSettings>>();
            var systemFactory = GetManager<ISystemFactory>();
            var tileGridFactory = GetManager<ITileGridFactory>();
            var broadphaseGridFactory = GetManager<IBroadphaseFactory>();
            var palettesDataProvider = GetManager<PalettesDataProvider>();
            var repositoryProvider = GetManager<IRepositoryProvider>();
            var mapLegacyLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();
            var mapTxtLoader = dataLoaderFactory.GetLoader<MapTxtDataLoader>();
            var builderFactory = GetManager<IBuilderFactory>();


            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            var palette = PaletteModel.NullPalette;//  palettesDataProvider.GetPalette(dbMap.PaletteRefs.First());

            //Load common sprites
            var dbSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbSpriteAtlas)
                loader.Load(dbAnim.Id, palette);

            var gameWorldBuilder = worldMan.Create();
            gameWorldBuilder.SetName("Dummy");

            var mapEntity = entityMan.Create($"Maps");

            var tileGridComponent = builderFactory.GetBuilder<TileGridComponentBuilder>()
                .SetGrid(width, height, 1, 16)
                .Build();

            var dataGridComponent = builderFactory.GetBuilder<DataGridComponentBuilder>()
                .SetGrid(width, height)
                .Build();

            mapEntity.Add(new StampPutterComponent());
            mapEntity.Add(tileGridComponent);
            mapEntity.Add(dataGridComponent);

            gameWorldBuilder.SetupGameWorldSystems();

            var gameWorld = gameWorldBuilder.Build();

            //var playerCamera = cameraHelper.CreateCamera(0, 0, 640, 480);
            var playerCamera = cameraHelper.CreateCamera("Camera.Player", 0, 0, 320, 240);

            playerCamera.Add(new PauseImmuneComponent());

            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
            gameViewport.SetViewportCamera(playerCamera.Id);

            //Follow John actor

            var player1Entity = entityMan.GetByTag("Players/P1").First();

            var johnPlayerEntity = actorHelper.CreatePlayerActor("John", new Vector2(0, 0));

            player1Entity.SetControlledEntity(johnPlayerEntity.Id);

            scriptMan.Expose("JohnPlayer", johnPlayerEntity);

            johnPlayerEntity.AddFollower(playerCamera);

            triggerMan.OnEntityFollow(johnPlayerEntity, (s, a) =>
            {
                var followerEntity = entityMan.GetById(a.FollowerId);
                Glue(johnPlayerEntity, followerEntity);
            });

            triggerMan.OnWorldInitialized(gameWorld, () =>
            {
                worldMan.RequestAddEntity(mapEntity, gameWorld.Id);

                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, playerCamera, gameWorld.Name, 0);
            });
        }

        private void InitLimboWorld()
        {
            var worldMan = GetManager<IWorldMan>();
            var worldBuilder = worldMan.Create();
            worldBuilder.SetName("Limbo");
            worldBuilder.SetupLimboWorldSystems();

            var gameWorld = worldBuilder.Build();
        }

        void OnRenderFrame(Rendering.Interface.Managers.IRenderView view, Matrix4 transform, float dt)
        {
            var screenWorld = worldMan.GetByName("ScreenWorld");

            if (screenWorld is null)
            {
                return;
            }

            var renderable = screenWorld.Systems.OfType<IRenderableSystem>().ToArray();
            var renderContext = new WorldRenderContext(view, 0, dt,new Box2(view.Box.Min, view.Box.Max), screenWorld);
            for (int i = 0; i < renderable.Length; i++)
            {
                renderable[i].Render(renderContext);
            }
        }

        private void OnWindowLoad(WindowLoadEvent e)
        {
            var renderView = e.RenderContext.CreateView(OnRenderFrame);

            var dataLoaderFactory = GetManager<IDataLoaderFactory>();

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
            var actorHelper = GetManager<ActorHelper>();
            var teleportHelper = GetManager<TeleportHelper>();
            var cameraHelper = GetManager<CameraHelper>();
            var entityMan = GetManager<IEntityMan>();
            var triggerMan = GetManager<ITriggerMan>();
            var screenWorldHelper = GetManager<ScreenWorldHelper>();
            var gameHudWorldHelper = GetManager<GameHudWorldHelper>();
            var debugHudWorldHelper = GetManager<DebugHudWorldHelper>();
            var smartCardScreenWorldHelper = GetManager<SmartcardScreenWorldHelper>();
            var missionScreenWorldHelper = GetManager<MissionScreenWorldHelper>();

            //Create 4 sound sources, each one acting as a separate channel
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();

            //var amfFilePath = @"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\TITLE.AMF";
            //var mod = OpenMod(amfFilePath);
            //var musicId = soundMan.CreateStream("MUSIC", (bufferSize, buffer) => ReadStream(mod, bufferSize, buffer));
            //soundMan.PlayStream(musicId);

            actorHelper.RegisterCollisionPairs();
            worldGateHelper.RegisterCollisionPairs();

            cameraHelper.CreateAnimations();

            var screenWorld = screenWorldHelper.CreateWorld(renderView);

            debugHudWorldHelper.Create();

            GetManager<IScriptMan>().Expose("Factory", GetManager<IEntityFactory>());

            //LoadSandboxWorld(40, 40);

            InitPlayers();
            InitLimboWorld();
            InitGameWorld();

            gameHudWorldHelper.Create();
            smartCardScreenWorldHelper.Create();
            missionScreenWorldHelper.Create();


            //var hudWorld = worldMan.GetByName(GameHudWorldHelper.WorldName);

            //triggerMan.OnWorldInitialized(hudWorld, () =>
            //{
            //    var smartcardReaderCameraEntity = entityMan.GetByTag("Camera.MissionScreen").First();

            //    var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            //    gameViewport.SetViewportCamera(smartcardReaderCameraEntity.Id);
            //}, singleTime: true);

            OnEngineInitialized();
            StartLuaConsoleInput();
        }

        private void StartLuaConsoleInput()
        {
            Task.Run(() => LuaConsoleInput(
                GetManager<IScriptMan>(),
                GetManager<CollisionVisualizingOptions>()));
        }

        private void InitLua()
        {
            //scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        #endregion Private Methods
    }
}