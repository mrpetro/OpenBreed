using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
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
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Button;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.Turret;
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
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Control;
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
using OpenTK.Input;
using System;
using System.Drawing;
using System.Reflection;

namespace OpenBreed.Sandbox
{
    public class ProgramFactory : CoreFactory
    {
        #region Public Constructors

        public ProgramFactory()
        {

            manCollection.AddSingleton<IViewClient>(() => new OpenTKWindowClient(800, 600, "OpenBreed"));

            manCollection.AddSingleton<IFsmMan>(() => new FsmMan());

            manCollection.AddSingleton<IAnimMan>(() => new AnimMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<EntityCommandHandler>(() => new EntityCommandHandler(manCollection.GetManager<IEntityMan>(),
                                                                                            manCollection.GetManager<IWorldMan>(),
                                                                                            manCollection.GetManager<ICommandsMan>(),
                                                                                            manCollection.GetManager<IEventsMan>()));

            manCollection.SetupLuaScripting();
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




            //manCollection.SetupAudioSystems();

            //manCollection.SetupGameScriptingApi();
        }

        #endregion Public Constructors

        #region Public Methods

        public ICore Create()
        {
            var core = new Program(manCollection, manCollection.GetManager<IViewClient>());

            manCollection.AddSingleton<ViewportCreator>(() => new ViewportCreator(core, manCollection.GetManager<IEntityMan>()));
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
            manCollection.AddSingleton<GameWorldHelper>(() => new GameWorldHelper(core, manCollection,
                                                                                        manCollection.GetManager<IPlayersMan>(),
                                                                                            manCollection.GetManager<ICommandsMan>(),
                                                                                            manCollection.GetManager<IEntityMan>(),
                                                                                        manCollection.GetManager<ISystemFactory>(),
                                                                                            manCollection.GetManager<IWorldMan>(),
                                                                                            manCollection.GetManager<IRenderingMan>(),
                                                                                            manCollection.GetManager<IEventsMan>()));

            manCollection.AddSingleton<WorldGateHelper>(() => new WorldGateHelper(core));
            manCollection.AddSingleton<DoorHelper>(() => new DoorHelper(core));
            manCollection.AddSingleton<ProjectileHelper>(() => new ProjectileHelper(core));
            manCollection.AddSingleton<ActorHelper>(() => new ActorHelper(core));

            return core;
        }

        #endregion Public Methods
    }

    public class Program : CoreBase
    {
        #region Private Fields

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
            Animations = manCollection.GetManager<IAnimMan>();
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
        public IAnimMan Animations { get; }

        public IFsmMan StateMachines { get; }

        public IPlayersMan Players { get; }

        public IWorldMan Worlds { get; }

        public IEntityMan Entities { get; }

        public override JobMan Jobs { get; }

        public IInputsMan Inputs { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Load()
        {
        }

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
            //var luaTest = new LoadTest();

            //luaTest.Example();

            var programFactory = new ProgramFactory();

            var program = programFactory.Create();

            //program.Sounds.Sounds.PlaySound(0);

            program.Run();
        }

        private void OnUpdateFrame(float dt)
        {
            GetManager<IEventQueue>().Fire();

            Commands.ExecuteEnqueued(this);

            Worlds.Cleanup();
            renderingMan.Cleanup();

            Players.ResetInputs();

            Inputs.Update();

            //StateMachine.Update((float)e.Time);
            Worlds.Update(this, dt);
            Jobs.Update(dt);
        }

        private void RegisterSystems()
        {
            TileSystem.RegisterHandlers(Commands);
            WalkingControlSystem.RegisterHandlers(Commands);
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

            RegisterSystems();
            RegisterShapes();
            RegisterFixtures();
            RegisterInputs();
            RegisterPlayers();

            var spriteMan = GetManager<ISpriteMan>();
            var tileMan = GetManager<ITileMan>();
            var textureMan = GetManager<ITextureMan>();

            var tileTex = textureMan.Create("Textures/Tiles/16/Test", @"Content\Graphics\TileAtlasTest32bit.bmp");
            tileMan.Create("Atlases/Tiles/16/Test", tileTex.Id, 16, 4, 4);

            var doorTex = textureMan.Create("Textures/Sprites/Door", @"Content\Graphics\DoorSpriteSet.png");
            spriteMan.Create("Atlases/Sprites/Door/Horizontal", doorTex.Id, 32, 16, 5, 1, 0, 0);
            spriteMan.Create("Atlases/Sprites/Door/Vertical", doorTex.Id, 16, 32, 5, 1, 0, 16);

            var teleportTex = textureMan.Create("Textures/Sprites/Teleport", @"Content\Graphics\TeleportSpriteSet.png");
            spriteMan.Create(TeleportHelper.SPRITE_TELEPORT_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 0);
            spriteMan.Create(TeleportHelper.SPRITE_TELEPORT_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 32);
            spriteMan.Create(WorldGateHelper.SPRITE_WORLD_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 96);
            spriteMan.Create(WorldGateHelper.SPRITE_WORLD_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 64);

            var laserTex = textureMan.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            spriteMan.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var arrowTex = textureMan.Create("Textures/Sprites/Arrow", @"Content\Graphics\ArrowSpriteSet.png");
            spriteMan.Create(ActorHelper.SPRITE_ARROW, arrowTex.Id, 32, 32, 8, 5);

            var turretTex = textureMan.Create("Textures/Sprites/Turret", @"Content\Graphics\TurretSpriteSet.png");
            spriteMan.Create(TurretHelper.SPRITE_TURRET, turretTex.Id, 32, 32, 8, 2);

            var cursorsTex = textureMan.Create("Textures/Sprites/Cursors", @"Content\Graphics\Cursors.png");
            spriteMan.Create("Atlases/Sprites/Cursors", cursorsTex.Id, 16, 16, 1, 1);

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
            doorHelper.CreateStamps();
            doorHelper.CreateAnimations();
            actorHelper.CreateAnimations();
            TurretHelper.CreateAnimations(this);
            TeleportHelper.CreateAnimations(this);
            projectileHelper.CreateAnimations();
            Misc.CreateAnimations(this);


            doorHelper.CreateFsm();
            projectileHelper.CreateFsm();
            ButtonHelper.CreateFsm(this);
            ActorAttackingHelper.CreateFsm(this);
            ActorMovementHelper.CreateFsm(this);
            //ActorRotationHelper.CreateFsm(this);
            TurretHelper.CreateRotationFsm(this);


            var screenWorldHelper = GetManager<ScreenWorldHelper>();

            renderingMan.ScreenWorld = screenWorldHelper.CreateWorld();
            //TextWorldHelper.Create(this);

            var hudWorldHelper = GetManager<HudWorldHelper>();
            hudWorldHelper.Create();

            var gameWorldHelper = GetManager<GameWorldHelper>();
            gameWorldHelper.Create();

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
            fixtureMan.Create("Fixtures/Turret", "Static", shapeMan.GetByTag("Shapes/Box_0_0_32_32"));
        }

        #endregion Private Methods
    }
}