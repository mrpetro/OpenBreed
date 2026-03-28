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
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Extensions;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Windows.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
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
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Loaders;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Animation.Components.Extensions;
using OpenBreed.Wecs.Audio.Components.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Gui.Systems;
using OpenBreed.Wecs.Gui.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Helpers;
using OpenBreed.Wecs.Rendering.Systems.Primitives;
using OpenBreed.Wecs.Scripting.Systems;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
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
using System.Windows.Documents;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            //hostBuilder.SetupLuaConsoleInput();

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

            hostBuilder.SetupGameHudWorldHelper();
            hostBuilder.SetupWeaponsMan();
            hostBuilder.SetupTeleportHelper();
            hostBuilder.SetupDynamicResolver();
            //hostBuilder.SetupWecsWorlds();

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
            var entityMan = serviceProvider.GetRequiredService<IEntityMan>();
            var scriptMan = serviceProvider.GetRequiredService<IScriptMan>();
            var gameServices = serviceProvider.GetRequiredService<IGameServices>();
            var tileMan = serviceProvider.GetRequiredService<ITileMan>();
            var triggerMan = serviceProvider.GetRequiredService<ITriggerMan>();
            var gameSettings = serviceProvider.GetRequiredService<IOptions<GameSettings>>();
            var entityFactory = serviceProvider.GetRequiredService<IEntityFactory>();


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


            triggerMan.OnWorldInitialized(gameWorld, () =>
            {
                var johnPlayerEntity = entityMan.GetByTag("John").FirstOrDefault();
                gameServices.ExecuteHeroEnter(johnPlayerEntity, gameWorld.Name, 0);
            });
        }

        void OnRenderFrame(Rendering.Abstractions.IRenderView view, float dt)
        {
            var worldMan = view.Context.ServiceProvider.GetRequiredService<IWorldMan>();

            var screenWorld = worldMan.GetByName(WorldNames.ScreenWorld);

            if (screenWorld is null)
            {
                return;
            }

            view.RenderWorld(screenWorld, 0, new Box2(view.Box.Min, view.Box.Max), dt);
        }

        private void OnWindowLoad(WindowLoadEvent e)
        {
            var renderView = e.RenderContext.CreateView();

            var sp = e.RenderContext.ServiceProvider;

            renderView.Rendering += OnRenderFrame;
            var dataLoaderFactory = sp.GetRequiredService<IDataLoaderFactory>();

            sp.GetRequiredService<FixtureTypes>().Register();
            sp.GetRequiredService<FontHelper>().SetupGameFont();

            var spriteMan = sp.GetRequiredService<ISpriteMan>();
            var worldMan = sp.GetRequiredService<IWorldMan>();
            var scriptMan = sp.GetRequiredService<IScriptMan>();
            var tileMan = sp.GetRequiredService<ITileMan>();
            var textureMan = sp.GetRequiredService<ITextureMan>();
            var soundMan = sp.GetRequiredService<ISoundMan>();
            var teleportHelper = sp.GetRequiredService<TeleportHelper>();
            var entityMan = sp.GetRequiredService<IEntityMan>();
            var triggerMan = sp.GetRequiredService<ITriggerMan>();
            var gameHudWorldHelper = sp.GetRequiredService<SetupHelper>();

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

            gameHudWorldHelper.Setup();

            worldMan.CreateScreenWorld();

            //LoadSandboxWorld(40, 40);

            InitPlayers(sp);
            worldMan.CreateLimboWorld();
            InitGameWorld(sp);
            worldMan.CreateDebugHud();
            worldMan.CreateGameHud();
            worldMan.CreateSmartCardReader();
            worldMan.CreateMissionScreen();

            //var hudWorld = worldMan.GetByName(GameHudWorldHelper.WorldName);

            //triggerMan.OnWorldInitialized(hudWorld, () =>
            //{
            //    var smartcardReaderCameraEntity = entityMan.GetByTag("Camera.MissionScreen").First();

            //    var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            //    gameViewport.SetViewportCamera(smartcardReaderCameraEntity.Id);
            //}, singleTime: true);

            OnEngineInitialized(sp);
        }

        #endregion Private Methods
    }
}