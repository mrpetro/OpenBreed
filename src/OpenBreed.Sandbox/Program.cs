using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Components.Physics;
using OpenBreed.Components.Physics.Xml;
using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Components.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Components.Xml;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Physics.Generic;
using OpenBreed.Core.Systems;
using OpenBreed.Physics.Interface;
using OpenBreed.Components.Rendering;
using OpenBreed.Components.Rendering.Xml;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Systems.Rendering;
using OpenBreed.Systems.Rendering.Builders;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Button;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.Turret;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Items;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Systems.Physics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Reflection;
using OpenBreed.Physics.Generic.Shapes;
using OpenBreed.Systems.Control.Builders;
using OpenBreed.Systems.Control.Systems;
using OpenBreed.Systems.Control;
using OpenBreed.Audio.Interface;

namespace OpenBreed.Sandbox
{
    public class ProgramFactory : CoreFactory
    {
        public ProgramFactory()
        {
            manCollection.AddSingleton<IScriptMan>(() => new LuaScriptMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IFsmMan>(() => new FsmMan());

            manCollection.AddSingleton<IAnimMan>(() => new AnimMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IInputsMan>(() => new InputsMan(manCollection.GetManager<ICore>()));

            manCollection.AddSingleton<IPlayersMan>(() => new PlayersMan(manCollection.GetManager<ILogger>(),
                                                                         manCollection.GetManager<IInputsMan>()));

            OpenGLModule.AddManagers(manCollection);
            PhysicsModule.AddManagers(manCollection);
        }

        public ICore Create()
        {
            return new Program(manCollection);
        }
    }

    public class Program : CoreBase
    {
        #region Private Fields

        internal VideoSystemsFactory VideoSystemsFactory { get; }
        internal PhysicsSystemsFactory PhysicsSystemsFactory { get; }


        private readonly IScriptMan scriptMan;
        private readonly LogConsolePrinter logConsolePrinter;
        private GameWindow window;

        private string appVersion;

        #endregion Private Fields

        #region Public Constructors

        public Program(IManagerCollection manCollection) :
            base(manCollection)
        {
            scriptMan = manCollection.GetManager<IScriptMan>();
            StateMachines = manCollection.GetManager<IFsmMan>();
            Animations = manCollection.GetManager<IAnimMan>();
            Inputs = manCollection.GetManager<IInputsMan>();
            Players = manCollection.GetManager<IPlayersMan>();

            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            window = new GameWindow(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed");

            window.MouseDown += (s, a) => Inputs.OnMouseDown(a);
            window.MouseUp += (s, a) => Inputs.OnMouseUp(a);
            window.MouseMove += (s, a) => Inputs.OnMouseMove(a);
            window.MouseWheel += (s, a) => Inputs.OnMouseWheel(a);
            window.KeyDown += (s, a) => Inputs.OnKeyDown(a);
            window.KeyUp += (s, a) => Inputs.OnKeyUp(a);
            window.KeyPress += (s, a) => Inputs.OnKeyPress(a);
            window.Load += (s, a) => OnWindowLoad();
            window.Resize += (s, a) => OnResize();
            window.UpdateFrame += (s, a) => Update((float)a.Time);
            window.RenderFrame += OnRenderFrame;

            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Jobs = new JobMan(this);

            EntityFactory = new EntityFactory(this);

            renderingModule = new OpenGLModule(this);
            Physics = new PhysicsModule(this);
            Sounds = new OpenALModule(this);

            VideoSystemsFactory = new VideoSystemsFactory(this);
            PhysicsSystemsFactory = new PhysicsSystemsFactory(this);


            RegisterModule<IRenderModule>(renderingModule);
            RegisterModule<IPhysicsModule>(Physics);
            RegisterModule<IAudioModule>(Sounds);

            window.VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Properties

        public IPhysicsModule Physics { get; }
        public IAudioModule Sounds { get; }

        public override EntityFactory EntityFactory { get; }

        private readonly IRenderModule renderingModule;

        public override IAnimMan Animations { get; }

        public override IFsmMan StateMachines { get; }

        public override IPlayersMan Players { get; }

        public override JobMan Jobs { get; }

        public override IInputsMan Inputs { get; }

        public override Matrix4 ClientTransform { get; protected set; }

        public override float ClientRatio { get { return (float)ClientRectangle.Width / (float)ClientRectangle.Height; } }

        public override Rectangle ClientRectangle => window.ClientRectangle;

        #endregion Public Properties

        #region Public Methods

        public WalkingControlSystemBuilder CreateWalkingControlSystem()
        {
            return new WalkingControlSystemBuilder(this);
        }

        public AiControlSystemBuilder CreateAiControlSystem()
        {
            return new AiControlSystemBuilder(this);
        }

        public PhysicsSystemBuilder CreatePhysicsSystem()
        {
            return new PhysicsSystemBuilder(this);
        }

        public AnimationSystemBuilder CreateAnimationSystem()
        {
            return new AnimationSystemBuilder(this);
        }

        public MovementSystemBuilder CreateMovementSystem()
        {
            return new MovementSystemBuilder(this);
        }

        public override void Load()
        {
        }

        public override void Update(float dt)
        {
            Commands.ExecuteEnqueued();

            Worlds.Cleanup();

            renderingModule.Cleanup();

            Players.ResetInputs();

            Inputs.Update();
            Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            Worlds.Update(dt);
            Jobs.Update(dt);
        }

        public override void Run()
        {
            window.Run(30.0, 60.0);
        }

        public override void Exit()
        {
            window.Exit();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnEngineInitialized()
        {
            scriptMan.TryInvokeFunction("EngineInitialized");
        }

        protected void OnRenderFrame(object sender, FrameEventArgs e)
        {
            renderingModule.Draw((float)e.Time);

            window.SwapBuffers();
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

        private void RegisterSystems()
        {
            FollowerSystem.RegisterHandlers(Commands);
            StateMachineSystem.RegisterHandlers(Commands);
            TextInputSystem.RegisterHandlers(Commands);
            TimerSystem.RegisterHandlers(Commands);
            SpriteSystem.RegisterHandlers(Commands);
            TileSystem.RegisterHandlers(Commands);
            TextPresenterSystem.RegisterHandlers(Commands);
            ViewportSystem.RegisterHandlers(Commands);
            AnimationSystem.RegisterHandlers(Commands);
            TextSystem.RegisterHandlers(Commands);
            WalkingControlSystem.RegisterHandlers(Commands);
            PhysicsSystem.RegisterHandlers(Commands);
        }

        private void OnWindowLoad()
        {
            window.Title = $"Open Breed Sandbox (Version: {appVersion} Vsync: {window.VSync})";

            RegisterXmlComponents();
            RegisterComponentFactories();

            ExposeScriptingApi();

            RegisterSystems();
            RegisterShapes();
            RegisterFixtures();

            Inputs.RegisterHandler(new WalkingControlHandler());
            Inputs.RegisterHandler(new AttackControlHandler());

            var p1 = Players.AddPlayer("P1");
            p1.RegisterInput(new AttackingPlayerInput());
            p1.RegisterInput(new WalkingPlayerInput());
            p1.AddKeyBinding("Attacking", "Primary", Key.ControlRight);
            p1.AddKeyBinding("Walking", "Left", Key.Left);
            p1.AddKeyBinding("Walking", "Right", Key.Right);
            p1.AddKeyBinding("Walking", "Up", Key.Up);
            p1.AddKeyBinding("Walking", "Down", Key.Down);

            var p2 = Players.AddPlayer("P2");
            p2.RegisterInput(new WalkingPlayerInput());
            p2.AddKeyBinding("Walking", "Left", Key.A);
            p2.AddKeyBinding("Walking", "Right", Key.D);
            p2.AddKeyBinding("Walking", "Up", Key.W);
            p2.AddKeyBinding("Walking", "Down", Key.S);

            var tileTex = renderingModule.Textures.Create("Textures/Tiles/16/Test", @"Content\Graphics\TileAtlasTest32bit.bmp");
            renderingModule.Tiles.Create("Atlases/Tiles/16/Test", tileTex.Id, 16, 4, 4);

            var doorTex = renderingModule.Textures.Create("Textures/Sprites/Door", @"Content\Graphics\DoorSpriteSet.png");
            renderingModule.Sprites.Create("Atlases/Sprites/Door/Horizontal", doorTex.Id, 32, 16, 5, 1, 0, 0);
            renderingModule.Sprites.Create("Atlases/Sprites/Door/Vertical", doorTex.Id, 16, 32, 5, 1, 0, 16);

            var teleportTex = renderingModule.Textures.Create("Textures/Sprites/Teleport", @"Content\Graphics\TeleportSpriteSet.png");
            renderingModule.Sprites.Create(TeleportHelper.SPRITE_TELEPORT_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 0);
            renderingModule.Sprites.Create(TeleportHelper.SPRITE_TELEPORT_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 32);
            renderingModule.Sprites.Create(WorldGateHelper.SPRITE_WORLD_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 96);
            renderingModule.Sprites.Create(WorldGateHelper.SPRITE_WORLD_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 64);

            var laserTex = renderingModule.Textures.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            renderingModule.Sprites.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var arrowTex = renderingModule.Textures.Create("Textures/Sprites/Arrow", @"Content\Graphics\ArrowSpriteSet.png");
            renderingModule.Sprites.Create(ActorHelper.SPRITE_ARROW, arrowTex.Id, 32, 32, 8, 5);

            var turretTex = renderingModule.Textures.Create("Textures/Sprites/Turret", @"Content\Graphics\TurretSpriteSet.png");
            renderingModule.Sprites.Create(TurretHelper.SPRITE_TURRET, turretTex.Id, 32, 32, 8, 2);

            var cursorsTex = renderingModule.Textures.Create("Textures/Sprites/Cursors", @"Content\Graphics\Cursors.png");
            renderingModule.Sprites.Create("Atlases/Sprites/Cursors", cursorsTex.Id, 16, 16, 1, 1);

            ColliderTypes.Initialize(Physics.Collisions);
            ActorHelper.RegisterCollisionPairs(this);
            WorldGateHelper.RegisterCollisionPairs(this);
            //TeleportHelper.RegisterCollisionPairs(this);
            ProjectileHelper.RegisterCollisionPairs(this);

            CameraHelper.CreateAnimations(this);
            DoorHelper.CreateStamps(this);
            DoorHelper.CreateAnimations(this);
            ActorHelper.CreateAnimations(this);
            TurretHelper.CreateAnimations(this);
            TeleportHelper.CreateAnimations(this);
            ProjectileHelper.CreateAnimations(this);
            Misc.CreateAnimations(this);

            DoorHelper.CreateFsm(this);
            ProjectileHelper.CreateFsm(this);
            ButtonHelper.CreateFsm(this);
            ActorAttackingHelper.CreateFsm(this);
            ActorMovementHelper.CreateFsm(this);
            //ActorRotationHelper.CreateFsm(this);
            TurretHelper.CreateRotationFsm(this);

            renderingModule.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            //TextWorldHelper.Create(this);
            HudWorldHelper.Create(this);
            GameWorldHelper.Create(this);

            OnEngineInitialized();
        }

        private void RegisterXmlComponents()
        {
            XmlComponentsList.RegisterComponentType<XmlPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlVelocityComponent>();
            XmlComponentsList.RegisterComponentType<XmlThrustComponent>();
            XmlComponentsList.RegisterComponentType<XmlSpriteComponent>();
            XmlComponentsList.RegisterComponentType<XmlTextComponent>();
            XmlComponentsList.RegisterComponentType<XmlAnimationComponent>();
            XmlComponentsList.RegisterComponentType<XmlBodyComponent>();
            XmlComponentsList.RegisterComponentType<XmlClassComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlMotionComponent>();
            XmlComponentsList.RegisterComponentType<XmlTimerComponent>();
            XmlComponentsList.RegisterComponentType<XmlFsmComponent>();
        }

        private void RegisterComponentFactories()
        {
            EntityFactory.RegisterComponentFactory<XmlPositionComponent>(new PositionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlVelocityComponent>(new VelocityComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlThrustComponent>(new ThrustComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlSpriteComponent>(new SpriteComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlTextComponent>(new TextComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlAnimationComponent>(new AnimationComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlBodyComponent>(new BodyComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlClassComponent>(new ClassComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(new AngularPositionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlMotionComponent>(new MotionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlTimerComponent>(new TimerComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlFsmComponent>(new FsmComponentFactory(this));
        }

        private void ExposeScriptingApi()
        {
            scriptMan.Expose("Worlds", Worlds);
            scriptMan.Expose("Entities", Entities);
            scriptMan.Expose("Commands", Commands);
            scriptMan.Expose("Inputs", Inputs);
            scriptMan.Expose("Logging", Logging);
            scriptMan.Expose("Players", Players);

            scriptMan.RunFile(@"Content\Scripts\start.lua");
        }

        private void OnResize()
        {
            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, ClientRectangle.Width, 0.0f, ClientRectangle.Height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Height, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(1.0f, -1.0f, 1.0f));
            renderingModule.OnClientResized(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void RegisterShapes()
        {
            Physics.Shapes.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
            Physics.Shapes.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
            Physics.Shapes.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
            Physics.Shapes.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
            Physics.Shapes.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
        }

        private void RegisterFixtures()
        {
            Physics.Fixturs.Create("Fixtures/GridCell", "Trigger", Physics.Shapes.GetByTag("Shapes/Box_0_0_16_16"));
            Physics.Fixturs.Create("Fixtures/TeleportEntry", "Trigger", Physics.Shapes.GetByTag("Shapes/Box_16_16_8_8"));
            Physics.Fixturs.Create("Fixtures/TeleportExit", "Trigger", Physics.Shapes.GetByTag("Shapes/Box_16_16_8_8"));
            Physics.Fixturs.Create("Fixtures/Projectile", "Dynamic", Physics.Shapes.GetByTag("Shapes/Box_0_0_16_16"));
            Physics.Fixturs.Create("Fixtures/DoorVertical", "Static", Physics.Shapes.GetByTag("Shapes/Box_0_0_16_32"));
            Physics.Fixturs.Create("Fixtures/DoorHorizontal", "Static", Physics.Shapes.GetByTag("Shapes/Box_0_0_32_16"));
            Physics.Fixturs.Create("Fixtures/Arrow", "Dynamic", Physics.Shapes.GetByTag("Shapes/Box_0_0_32_32"));
            Physics.Fixturs.Create("Fixtures/Turret", "Static", Physics.Shapes.GetByTag("Shapes/Box_0_0_32_32"));
        }

        #endregion Private Methods
    }
}