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
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Common.Game.Wecs.Components;
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
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Game.Extensions;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Wecs.Systems.Rendering.Helpers;

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

        public ICore Create()
        {
            var appName = ProgramTools.AppProductName;
            var infoVersion = ProgramTools.AppInfoVerion;

            hostBuilder.SetupDataHandlers();

            hostBuilder.SetupDataGridFactory();

            hostBuilder.SetupGameWindow(640, 480, $"{appName} v{infoVersion}");
            hostBuilder.SetupGLWindow();
            hostBuilder.SetupWindowsDrawingContext();

            hostBuilder.ConfigureServices(sc =>
            {
                sc.AddSingleton<CoordsTransformer>();
            });

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

            hostBuilder.SetupSandboxSystems();
            hostBuilder.SetupCommonGameServices(isEditor: false);
            hostBuilder.SetupCommonGameWecsServices(isEditor: false);

            hostBuilder.SetupWecsSandboxComponents();

            hostBuilder.SetupItemManager((itemsMap, sp) =>
            {
                itemsMap.RegisterAbtaItems();
            });

            hostBuilder.SetupViewportCreator();

            hostBuilder.SetupDataLoaderFactory((dataLoaderFactory, sp) =>
            {
                dataLoaderFactory.RegisterGraphicsDataLoader(sp);
                dataLoaderFactory.SetupAnimationDataLoader<IEntity>(sp);
                dataLoaderFactory.SetupMapLegacyDataLoader(sp);
                dataLoaderFactory.SetupSoundSampleDataLoader(sp);
                dataLoaderFactory.SetupScriptDataLoader(sp);
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

            hostBuilder.ConfigureServices((sc) => sc.AddScoped<FontHelper>());
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
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public Program(IHost host) :
            base(host)
        {
            host.RunAsync();

            window = host.Services.GetService<IWindow>();
            eventsMan = host.Services.GetService<IEventsMan>();

            eventsMan.Subscribe<WindowUpdateEvent>(OnUpdateFrame);
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

        protected void OnEngineInitialized(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IScriptMan>().TryInvokeFunction("EngineInitialized");
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
            ThreadTools.Initialize();

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

        private void OnUpdateFrame(WindowUpdateEvent e)
        {
            var dt = e.Dt;

            var sp = e.Window.Context.ServiceProvider;


            sp.GetRequiredService<IRenderingMan>().Update(dt);

            dt = Math.Min(1.0f/30.0f, dt);

            sp.GetRequiredService<IInputsMan>().Update();

            sp.GetRequiredService<IWorldMan>().Update(dt);

            sp.GetRequiredService<IJobsMan>().Update(dt);

            sp.GetRequiredService<ISoundMan>().Update();

            sp.GetRequiredService<IEntityMan>().Cleanup();
        }

        private int ReadStream(InterleavedStereoModule module, int bufferSize, short[] buffer)
        {
            return 2 * module.Read(48000, bufferSize / 2, buffer);
        }

        private InterleavedStereoModule OpenMod(string moduleFilePath)
        {
            return new InterleavedStereoModule(moduleFilePath);
        }

        private void InitPlayers(IServiceProvider serviceProvider)
        {

        }

        private void InitGameWorld(IServiceProvider serviceProvider)
        {
            var dataLoaderFactory = serviceProvider.GetRequiredService<IDataLoaderFactory>();
            var cameraHelper = serviceProvider.GetRequiredService<CameraHelper>();
            var entityMan = serviceProvider.GetRequiredService<IEntityMan>();
            var actorHelper = serviceProvider.GetRequiredService<ActorHelper>();
            var scriptMan = serviceProvider.GetRequiredService<IScriptMan>();
            var tileMan = serviceProvider.GetRequiredService<ITileMan>();
            var triggerMan = serviceProvider.GetRequiredService<ITriggerMan>();
            var worldGateHelper = serviceProvider.GetRequiredService<EntriesHelper>();
            var gameSettings = serviceProvider.GetRequiredService<IOptions<GameSettings>>();
            var hudHelper = serviceProvider.GetRequiredService<HudHelper>();

            var mapLegacyLoader = dataLoaderFactory.GetLoader<MapLegacyDataLoader>();

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
                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, gameWorld.Name, 0);
            });
        }

        private void Glue(IEntity followed, IEntity follower)
        {
            var followedPos = followed.Get<PositionComponent>();
            var followerPos = follower.Get<PositionComponent>();

            followerPos.Value = followedPos.Value;
        }

        private void InitLimboWorld(IServiceProvider serviceProvider)
        {
            var worldMan = serviceProvider.GetRequiredService<IWorldMan>();
            var worldBuilder = worldMan.Create();
            worldBuilder.SetName("Limbo");
            worldBuilder.SetupLimboWorldSystems();

            var gameWorld = worldBuilder.Build();
        }

        void OnRenderFrame(Rendering.Abstractions.IRenderView view, float dt)
        {
            var worldMan = view.Context.ServiceProvider.GetRequiredService<IWorldMan>();

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
            var renderView = e.RenderContext.CreateView();

            var sp = e.RenderContext.ServiceProvider;

            renderView.Rendering += OnRenderFrame;
            var dataLoaderFactory = sp.GetRequiredService<IDataLoaderFactory>();

            InitLua();

            sp.GetRequiredService<FixtureTypes>().Register();
            sp.GetRequiredService<FontHelper>().SetupGameFont();

            var spriteMan = sp.GetRequiredService<ISpriteMan>();
            var worldMan = sp.GetRequiredService<IWorldMan>();
            var scriptMan = sp.GetRequiredService<IScriptMan>();
            var tileMan = sp.GetRequiredService<ITileMan>();
            var textureMan = sp.GetRequiredService<ITextureMan>();
            var soundMan = sp.GetRequiredService<ISoundMan>();
            var worldGateHelper = sp.GetRequiredService<EntriesHelper>();
            var doorHelper = sp.GetRequiredService<DoorHelper>();
            var electicGateHelper = sp.GetRequiredService<ElectricGateHelper>();
            var pickableHelper = sp.GetRequiredService<PickableHelper>();
            var environmentHelper = sp.GetRequiredService<EnvironmentHelper>();
            var actorHelper = sp.GetRequiredService<ActorHelper>();
            var teleportHelper = sp.GetRequiredService<TeleportHelper>();
            var cameraHelper = sp.GetRequiredService<CameraHelper>();
            var entityMan = sp.GetRequiredService<IEntityMan>();
            var triggerMan = sp.GetRequiredService<ITriggerMan>();
            var screenWorldHelper = sp.GetRequiredService<ScreenWorldHelper>();
            var gameHudWorldHelper = sp.GetRequiredService<GameHudWorldHelper>();
            var debugHudWorldHelper = sp.GetRequiredService<DebugHudWorldHelper>();
            var smartCardScreenWorldHelper = sp.GetRequiredService<SmartcardScreenWorldHelper>();
            var missionScreenWorldHelper = sp.GetRequiredService<MissionScreenWorldHelper>();

            //Create 4 sound sources, each one acting as a separate channel
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();
            soundMan.CreateSoundSource();

            //var amfFilePath = @"D:\Games\Alien Breed Tower Assault Enhanced (1994)(Psygnosis Team 17)\extract\TITLE.AMF";
            //var mod = OpenMod(amfFilePath);
            //var musicId = soundMan.CreateStream("MUSIC", (bufferSize, buffer) => ReadStream(mod, bufferSize, buffer));
            //soundMan.PlayStream(musicId);

            //worldGateHelper.RegisterCollisionPairs();

            cameraHelper.CreateAnimations();

            var screenWorld = screenWorldHelper.CreateWorld(renderView);

            debugHudWorldHelper.Create();

            sp.GetRequiredService<IScriptMan>().Expose("Factory", sp.GetRequiredService<IEntityFactory>());

            //LoadSandboxWorld(40, 40);

            InitPlayers(sp);
            InitLimboWorld(sp);
            InitGameWorld(sp);

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

            OnEngineInitialized(sp);
            StartLuaConsoleInput(sp);
        }

        private void StartLuaConsoleInput(IServiceProvider serviceProvider)
        {
            System.Threading.Tasks.Task.Run(() => LuaConsoleInput(
                serviceProvider.GetRequiredService<IScriptMan>(),
                serviceProvider.GetRequiredService<CollisionVisualizingOptions>()));
        }

        private void InitLua()
        {
            //scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        #endregion Private Methods
    }
}