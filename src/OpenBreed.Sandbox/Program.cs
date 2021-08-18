using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Fsm.Xml;
using OpenBreed.Game;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Button;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.Turret;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Animation.Xml;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Physics.Xml;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Rendering.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Control.Handlers;
using OpenBreed.Wecs.Systems.Control.Inputs;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenBreed.Sandbox
{
    public class ProgramFactory : CoreFactory
    {
        #region Public Constructors

        public ProgramFactory()
        {
            manCollection.AddSingleton<IViewClient>(() => new OpenTKWindowClient(800, 600, "OpenBreed"));

            manCollection.SetupVariableManager();
            manCollection.SetupABFormats();
            manCollection.SetupAnimationManagers();
            manCollection.SetupModelProvider();
            manCollection.SetupLuaScripting();
            manCollection.SetupDataProviders();
            manCollection.SetupGenericInputManagers();
            manCollection.SetupGenericPhysicsManagers();
            manCollection.SetupOpenALManagers();
            manCollection.SetupOpenGLManagers();
            manCollection.SetupWecsManagers();
            manCollection.SetupRenderingSystems();
            manCollection.SetupPhysicsSystems();
            manCollection.SetupCoreSystems();
            manCollection.SetupControlSystems();
            manCollection.SetupAnimationSystems();
            manCollection.SetupGuiSystems();


            manCollection.SetupCommonComponents();
            manCollection.SetupPhysicsComponents();
            manCollection.SetupRenderingComponents();
            manCollection.SetupAnimationComponents();
            manCollection.SetupFsmComponents();

            manCollection.SetupMapEntityFactory();
            manCollection.SetupDataLoaderFactory();

            //manCollection.SetupAudioSystems();

            //manCollection.SetupGameScriptingApi();


            manCollection.SetupSandboxBuilders();

            manCollection.AddSingleton<ViewportCreator>(() => new ViewportCreator(manCollection.GetManager<IEntityMan>(), manCollection.GetManager<IEntityFactory>()));
        }



        #endregion Public Constructors

        #region Public Methods

        public ICore Create(string gameDbFilePath, string gameFolderPath)
        {
            var core = new Program(manCollection, manCollection.GetManager<IViewClient>());

            manCollection.AddSingleton<ScreenWorldHelper>(() => new ScreenWorldHelper(core, manCollection.GetManager<ISystemFactory>(),
                                                                                            manCollection.GetManager<ICommandsMan>(), 
                                                                                            manCollection.GetManager<IRenderingMan>(),
                                                                                            manCollection.GetManager<IWorldMan>(), 
                                                                                            manCollection.GetManager<IEventsMan>(),
                                                                                            manCollection.GetManager<ViewportCreator>(),
                                                                                            manCollection.GetManager<IViewClient>()));
            manCollection.AddSingleton<HudWorldHelper>(() => new HudWorldHelper(core, manCollection.GetManager<ISystemFactory>(),
                                                                                            manCollection.GetManager<IWorldMan>(),
                                                                                            manCollection.GetManager<IViewClient>(),
                                                                                            manCollection.GetManager<IEntityMan>()));
            manCollection.AddSingleton<GameWorldHelper>(() => new GameWorldHelper(manCollection,
                                                                                        manCollection.GetManager<IPlayersMan>(),
                                                                                            manCollection.GetManager<ICommandsMan>(),
                                                                                            manCollection.GetManager<IEntityMan>(),
                                                                                        manCollection.GetManager<ISystemFactory>(),
                                                                                            manCollection.GetManager<IWorldMan>(),
                                                                                            manCollection.GetManager<IRenderingMan>(),
                                                                                            manCollection.GetManager<IEventsMan>(),
                                                                                            manCollection.GetManager<ILogger>(),
                                                                                            manCollection.GetManager<ViewportCreator>()));

            manCollection.AddSingleton<WorldGateHelper>(() => new WorldGateHelper(core,
                                                                                  manCollection.GetManager<IWorldMan>(),
                                                                                  manCollection.GetManager<IEntityMan>(),
                                                                                  manCollection.GetManager<ViewportCreator>()));
            manCollection.AddSingleton<DoorHelper>(() => new DoorHelper(core));
            manCollection.AddSingleton<ProjectileHelper>(() => new ProjectileHelper(core));
            manCollection.AddSingleton<ActorHelper>(() => new ActorHelper(core, manCollection.GetManager<ICommandsMan>(),
                                                                                manCollection.GetManager<MapCellHelper>()));
            manCollection.AddSingleton<MapCellHelper>(() => new MapCellHelper(manCollection.GetManager<WorldBlockBuilder>(),
                                                                              manCollection.GetManager<ICommandsMan>()));

            manCollection.SetupSpriteComponentAnimator();

            var variables = manCollection.GetManager<IVariableMan>();

            variables.RegisterVariable(typeof(string), gameFolderPath, "Cfg.Options.ABTA.GameFolderPath");

            manCollection.AddSingleton<IRepositoryProvider>(() => new XmlReadonlyDatabaseMan(variables, gameDbFilePath));


            return core;
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
        //private GameWindow window;

        private string appVersion;

        #endregion Private Fields

        #region Public Constructors

        public Program(IManagerCollection manCollection, IViewClient clientMan) :
            base(manCollection)
        {
            this.clientMan = clientMan;
            scriptMan = manCollection.GetManager<IScriptMan>();
            StateMachines = manCollection.GetManager<IFsmMan>();
            Animations = manCollection.GetManager<Animation.Interface.IClipMan>();
            Inputs = manCollection.GetManager<IInputsMan>();
            renderingMan = manCollection.GetManager<IRenderingMan>();

            //clientMan.KeyDownEvent += (s, a) => Inputs.OnKeyDown(a);
            //clientMan.KeyUpEvent += (s, a) => Inputs.OnKeyUp(a);
            //clientMan.KeyPressEvent += (s, a) => Inputs.OnKeyPress(a);
            //clientMan.MouseMoveEvent += (s, a) => Inputs.OnMouseMove(a);
            //clientMan.MouseDownEvent += (s, a) => Inputs.OnMouseDown(a);
            //clientMan.MouseUpEvent += (s, a) => Inputs.OnMouseUp(a);
            //clientMan.MouseWheelEvent += (s, a) => Inputs.OnMouseWheel(a);

            Players = manCollection.GetManager<IPlayersMan>();
            Worlds = manCollection.GetManager<IWorldMan>();
            EntityFactory = manCollection.GetManager<IEntityFactory>();

            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            clientMan.UpdateFrameEvent += (s, a) => OnUpdateFrame(a);
            clientMan.LoadEvent += (s, a) => OnLoad();

            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Jobs = new JobMan(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntityFactory EntityFactory { get; }
        public Animation.Interface.IClipMan Animations { get; }

        public IFsmMan StateMachines { get; }

        public IPlayersMan Players { get; }

        public IWorldMan Worlds { get; }

        public IEntityMan Entities { get; }

        public override JobMan Jobs { get; }

        public IInputsMan Inputs { get; }

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


            var programFactory = new ProgramFactory();

            var program = programFactory.Create(gameDbFilePath, vanillaGameFolderPath);

            //program.Sounds.Sounds.PlaySound(0);

            program.Run();
        }

        private void OnUpdateFrame(float dt)
        {
            GetManager<IEventQueue>().Fire();

            Commands.ExecuteEnqueued();

            Worlds.Cleanup();
            renderingMan.Cleanup();

            Players.ResetInputs();

            Inputs.Update();

            //StateMachine.Update((float)e.Time);
            Worlds.Update(dt);
            Jobs.Update(dt);
        }

        private void RegisterInputs()
        {
            Inputs.RegisterHandler(new DigitalJoyInputHandler());
            Inputs.RegisterHandler(new ButtonInputHandler());
        }

        private void RegisterPlayers()
        {
            var p1 = Players.AddPlayer("P1");
            p1.RegisterInput(new ButtonPlayerInput());
            p1.RegisterInput(new DigitalJoyPlayerInput());
            p1.AddKeyBinding("Attacking", "Primary", Key.ControlRight);
            p1.AddKeyBinding("Walking", "Left", Key.Left);
            p1.AddKeyBinding("Walking", "Right", Key.Right);
            p1.AddKeyBinding("Walking", "Up", Key.Up);
            p1.AddKeyBinding("Walking", "Down", Key.Down);

            var p2 = Players.AddPlayer("P2");
            p2.RegisterInput(new DigitalJoyPlayerInput());
            p2.AddKeyBinding("Walking", "Left", Key.A);
            p2.AddKeyBinding("Walking", "Right", Key.D);
            p2.AddKeyBinding("Walking", "Up", Key.W);
            p2.AddKeyBinding("Walking", "Down", Key.S);
        }

        private void OnLoad()
        {
            //Client.Title = $"Open Breed Sandbox (Version: {appVersion} Vsync: {window.VSync})";

            InitLua();

            RegisterShapes();
            RegisterFixtures();
            RegisterInputs();
            RegisterPlayers();


            //var map = manCollection.GetManager<MapsDataProvider>().GetMap("CRASH LANDING SITE");



            var spriteMan = GetManager<ISpriteMan>();
            var tileMan = GetManager<ITileMan>();
            var textureMan = GetManager<ITextureMan>();

            var tileTex = textureMan.Create("Textures/Tiles/16/Test", @"Content\Graphics\TileAtlasTest32bit.bmp");
            tileMan.Create("Atlases/Tiles/16/Test", tileTex.Id, 16, 4, 4);

            var teleportTex = textureMan.Create("Textures/Sprites/Teleport", @"Content\Graphics\TeleportSpriteSet.png");
            spriteMan.CreateAtlas()
                .SetTexture(teleportTex.Id)
                .SetName(TeleportHelper.SPRITE_TELEPORT_ENTRY)
                .AppendCoordsFromGrid(32, 32, 4, 1, 0, 0)
                .Build();
            //spriteMan.Create(TeleportHelper.SPRITE_TELEPORT_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 0);
            spriteMan.CreateAtlas()
                .SetTexture(teleportTex.Id)
                .SetName(TeleportHelper.SPRITE_TELEPORT_EXIT)
                .AppendCoordsFromGrid(32, 32, 4, 1, 0, 32)
                .Build();
            //spriteMan.Create(TeleportHelper.SPRITE_TELEPORT_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 32);
            spriteMan.CreateAtlas()
                .SetTexture(teleportTex.Id)
                .SetName(WorldGateHelper.SPRITE_WORLD_ENTRY)
                .AppendCoordsFromGrid(32, 32, 4, 1, 0, 96)
                .Build();
            //spriteMan.Create(WorldGateHelper.SPRITE_WORLD_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 96);
            spriteMan.CreateAtlas()
                .SetTexture(teleportTex.Id)
                .SetName(WorldGateHelper.SPRITE_WORLD_EXIT)
                .AppendCoordsFromGrid(32, 32, 4, 1, 0, 64)
                .Build();
            //spriteMan.Create(WorldGateHelper.SPRITE_WORLD_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 64);

            var laserTex = textureMan.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            spriteMan.CreateAtlas()
                .SetTexture(laserTex.Id)
                .SetName("Atlases/Sprites/Projectiles/Laser")
                .AppendCoordsFromGrid(16, 16, 8, 1, 0, 0)
                .Build();
            //spriteMan.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var turretTex = textureMan.Create("Textures/Sprites/Turret", @"Content\Graphics\TurretSpriteSet.png");
            spriteMan.CreateAtlas()
                .SetTexture(turretTex.Id)
                .SetName(TurretHelper.SPRITE_TURRET)
                .AppendCoordsFromGrid(32, 32, 8, 2)
                .Build();

            //spriteMan.Create(TurretHelper.SPRITE_TURRET, turretTex.Id, 32, 32, 8, 2);

            var cursorsTex = textureMan.Create("Textures/Sprites/Cursors", @"Content\Graphics\Cursors.png");
            spriteMan.CreateAtlas()
                .SetTexture(cursorsTex.Id)
                .SetName("Atlases/Sprites/Cursors")
                .AppendCoordsFromGrid(16, 16, 1, 1)
                .Build();
            //spriteMan.Create("Atlases/Sprites/Cursors", cursorsTex.Id, 16, 16, 1, 1);

            var worldGateHelper = GetManager<WorldGateHelper>();
            var doorHelper = GetManager<DoorHelper>();
            var projectileHelper = GetManager<ProjectileHelper>();
            var actorHelper = GetManager<ActorHelper>();


            ColliderTypes.Initialize(GetManager<ICollisionMan>());
            actorHelper.RegisterCollisionPairs();
            worldGateHelper.RegisterCollisionPairs();
            //TeleportHelper.RegisterCollisionPairs(this);
            projectileHelper.RegisterCollisionPairs();

            CameraHelper.CreateAnimations(this);

            doorHelper.CreateAnimations();
            actorHelper.CreateAnimations();
            TurretHelper.CreateAnimations(this);
            TeleportHelper.CreateAnimations(this);
            projectileHelper.CreateAnimations();
            Misc.CreateAnimations(this);

            manCollection.SetupButtonStates();
            manCollection.SetupProjectileStates();
            manCollection.SetupDoorStates();
            manCollection.SetupActorAttackingStates();
            manCollection.SetupActorMovementStates();
            //manCollection.SetupActorRotationStates();
            manCollection.CreateTurretRotationStates();

            var screenWorldHelper = GetManager<ScreenWorldHelper>();

            renderingMan.ScreenWorld = screenWorldHelper.CreateWorld();
            //TextWorldHelper.Create(this);

            var hudWorldHelper = GetManager<HudWorldHelper>();
            hudWorldHelper.Create(this);


            var dataLoaderFactory = GetManager<IDataLoaderFactory>();
            var mapWorldLoader = dataLoaderFactory.GetLoader<World>();


            var cameraBuilder = GetManager<CameraBuilder>();
            var commandsMan = GetManager<ICommandsMan>();
            var entityMan = GetManager<IEntityMan>();

            var gameWorld = mapWorldLoader.Load("CIVILIAN ZONE 1");
            doorHelper.LoadStamps();

            cameraBuilder.SetupPlayerCamera();

            var playerCamera = cameraBuilder.Build();
            playerCamera.Tag = "PlayerCamera";
            var gameViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_VIEWPORT).First();
            gameViewport.Get<ViewportComponent>().CameraEntityId = playerCamera.Id;

            //Follow John actor
            //var johnPlayerEntity = entityMan.GetByTag("John").First();
            var johnPlayerEntity = actorHelper.CreatePlayerActor(new Vector2(0, 0));
            johnPlayerEntity.Tag = "John";

            commandsMan.Post(new FollowedAddFollowerCommand(johnPlayerEntity.Id, playerCamera.Id));


            GetManager<IEventsMan>().Subscribe<WorldInitializedEventArgs>(GetManager<IWorldMan>(), (s, a) =>
            {
                if (a.WorldId != gameWorld.Id)
                    return;

                worldGateHelper.ExecuteHeroEnter(johnPlayerEntity, gameWorld.Id, 0);

            });

            //var gameWorldHelper = GetManager<GameWorldHelper>();
            //gameWorldHelper.Create(this);

            OnEngineInitialized();
        }

        private void InitLua()
        {
            //scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        private void RegisterShapes()
        {
            var shapeMan = GetManager<IShapeMan>();

            shapeMan.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
            shapeMan.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
            shapeMan.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
            shapeMan.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
            shapeMan.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
            shapeMan.Register("Shapes/Box_0_0_28_28", new BoxShape(0, 0, 28, 28));
        }

        private void RegisterFixtures()
        {
            var shapeMan = GetManager<IShapeMan>();
            var fixtureMan = GetManager<IFixtureMan>();

            fixtureMan.Create("Fixtures/GridCell", "Trigger", shapeMan.GetByTag("Shapes/Box_0_0_16_16"));
            fixtureMan.Create("Fixtures/TeleportEntry", "Trigger", shapeMan.GetByTag("Shapes/Box_16_16_8_8"));
            fixtureMan.Create("Fixtures/TeleportExit", "Trigger", shapeMan.GetByTag("Shapes/Box_16_16_8_8"));
            fixtureMan.Create("Fixtures/Projectile", "Dynamic", shapeMan.GetByTag("Shapes/Box_0_0_16_16"));
            fixtureMan.Create("Fixtures/DoorVertical", "Static", shapeMan.GetByTag("Shapes/Box_0_0_16_32"));
            fixtureMan.Create("Fixtures/DoorHorizontal", "Static", shapeMan.GetByTag("Shapes/Box_0_0_32_16"));
            fixtureMan.Create("Fixtures/Arrow", "Dynamic", shapeMan.GetByTag("Shapes/Box_0_0_32_32"));
            fixtureMan.Create("Fixtures/Hero", "Dynamic", shapeMan.GetByTag("Shapes/Box_0_0_28_28"));
            fixtureMan.Create("Fixtures/Turret", "Static", shapeMan.GetByTag("Shapes/Box_0_0_32_32"));
        }

        #endregion Private Methods
    }
}