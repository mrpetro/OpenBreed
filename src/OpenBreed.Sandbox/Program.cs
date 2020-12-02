using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Builders;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Audio.Builders;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Control.Systems;
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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using OpenBreed.Core.Modules.Physics.Shapes;

namespace OpenBreed.Sandbox
{
    public class Program : CoreBase
    {
        private GameWindow window;

        #region Private Fields

        private string appVersion;

        #endregion Private Fields

        #region Public Constructors

        //private 

        private readonly LogConsolePrinter logConsolePrinter;


        public Program()
        {
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            window = new GameWindow(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed");

            window.MouseDown += (s,a) => Inputs.OnMouseDown(a);
            window.MouseUp += (s, a) => Inputs.OnMouseUp(a);
            window.MouseMove += (s, a) => Inputs.OnMouseMove(a);
            window.MouseWheel += (s, a) => Inputs.OnMouseWheel(a);
            window.KeyDown += (s, a) => Inputs.OnKeyDown(a);
            window.KeyUp += (s, a) => Inputs.OnKeyUp(a);
            window.KeyPress += (s, a) => Inputs.OnKeyPress(a);
            window.Load += (s,a) => OnWindowLoad();
            window.Resize += (s, a) => OnResize();
            window.UpdateFrame += (s,a) => Update((float)a.Time);
            window.RenderFrame += OnRenderFrame;

            Logging = new DefaultLogger();
            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();
            Scripts = new LuaScriptMan(this);
            StateMachines = new FsmMan(this);
            Players = new PlayersMan(this);
            Items = new ItemsMan(this);
            Inputs = new InputsMan(this);

            Jobs = new JobMan(this);

            Rendering = new OpenGLModule(this);
            Physics = new PhysicsModule(this);
            Sounds = new OpenALModule(this);
            Animations = new AnimMan(this);

            RegisterModule(Rendering);
            RegisterModule(Physics);
            RegisterModule(Sounds);


            window.VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IRenderModule Rendering { get; }

        public PhysicsModule Physics { get; }

        public override IAudioModule Sounds { get; }

        public override AnimMan Animations { get; }

        public override FsmMan StateMachines { get; }

        public override PlayersMan Players { get; }

        public override ILogger Logging { get; }

        public override JobMan Jobs { get; }

        public override ItemsMan Items { get; }

        public override InputsMan Inputs { get; }

        public override IScriptMan Scripts { get; }

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

        public TileSystemBuilder CreateTileSystem()
        {
            return new TileSystemBuilder(this);
        }

        public SpriteSystemBuilder CreateSpriteSystem()
        {
            return new SpriteSystemBuilder(this);
        }

        public TextSystemBuilder CreateTextSystem()
        {
            return new TextSystemBuilder(this);
        }

        //public WireframeSystemBuilder CreateWireframeSystem()
        //{
        //    return new WireframeSystemBuilder(this);
        //}

        public SoundSystemBuilder CreateSoundSystem()
        {
            return new SoundSystemBuilder(this);
        }



        #endregion Public Methods

        #region Protected Methods

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

            ExposeScriptingApi();

            RegisterSystems();
            RegisterComponentBuilders();
            RegisterShapes();
            RegisterFixtures();
            RegisterEntityTemplates();
            RegisterItems();

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

            var tileTex = Rendering.Textures.Create("Textures/Tiles/16/Test", @"Content\Graphics\TileAtlasTest32bit.bmp");
            Rendering.Tiles.Create("Atlases/Tiles/16/Test", tileTex.Id, 16, 4, 4);

            var doorTex = Rendering.Textures.Create("Textures/Sprites/Door", @"Content\Graphics\DoorSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Door/Horizontal", doorTex.Id, 32, 16, 5, 1, 0, 0);
            Rendering.Sprites.Create("Atlases/Sprites/Door/Vertical", doorTex.Id, 16, 32, 5, 1, 0, 16);

            var teleportTex = Rendering.Textures.Create("Textures/Sprites/Teleport", @"Content\Graphics\TeleportSpriteSet.png");
            Rendering.Sprites.Create(TeleportHelper.SPRITE_TELEPORT_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 0);
            Rendering.Sprites.Create(TeleportHelper.SPRITE_TELEPORT_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 32);
            Rendering.Sprites.Create(WorldGateHelper.SPRITE_WORLD_ENTRY, teleportTex.Id, 32, 32, 4, 1, 0, 96);
            Rendering.Sprites.Create(WorldGateHelper.SPRITE_WORLD_EXIT, teleportTex.Id, 32, 32, 4, 1, 0, 64);

            var laserTex = Rendering.Textures.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var arrowTex = Rendering.Textures.Create("Textures/Sprites/Arrow", @"Content\Graphics\ArrowSpriteSet.png");
            Rendering.Sprites.Create(ActorHelper.SPRITE_ARROW, arrowTex.Id, 32, 32, 8, 5);

            var turretTex = Rendering.Textures.Create("Textures/Sprites/Turret", @"Content\Graphics\TurretSpriteSet.png");
            Rendering.Sprites.Create(TurretHelper.SPRITE_TURRET, turretTex.Id, 32, 32, 8, 2);

            var cursorsTex = Rendering.Textures.Create("Textures/Sprites/Cursors", @"Content\Graphics\Cursors.png");
            Rendering.Sprites.Create("Atlases/Sprites/Cursors", cursorsTex.Id, 16, 16, 1, 1);

            ColliderTypes.Initialize(Physics.Collisions);
            ActorHelper.RegisterCollisionPairs(this);
            WorldGateHelper.RegisterCollisionPairs(this);
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

            Rendering.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            //TextWorldHelper.Create(this);
            HudWorldHelper.Create(this);
            GameWorldHelper.Create(this);

            OnEngineInitialized();
        }

        private void ExposeScriptingApi()
        {
            Scripts.Expose("Worlds", Worlds);
            Scripts.Expose("Entities", Entities);
            Scripts.Expose("Commands", Commands);
            Scripts.Expose("Inputs", Inputs);
            Scripts.Expose("Logging", Logging);
            Scripts.Expose("Players", Players);

            Scripts.RunFile(@"Content\Scripts\start.lua");
        }

        protected void OnEngineInitialized()
        {
            Scripts.TryInvokeFunction("EngineInitialized");
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
            Rendering.OnClientResized(ClientRectangle.Width, ClientRectangle.Height);
        }

        public override void Load()
        {
        }

        public override void Update(float dt)
        {
            Commands.ExecuteEnqueued();

            Worlds.Cleanup();

            Rendering.Cleanup();

            Players.ResetInputs();

            Inputs.Update();
            Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            Worlds.Update(dt);
            Jobs.Update(dt);
        }

        protected void OnRenderFrame(object sender, FrameEventArgs e)
        {
            Rendering.Draw((float)e.Time);

            window.SwapBuffers();
        }

        #endregion Protected Methods

        #region Private Methods

        [STAThread]
        private static void Main(string[] args)
        {
            //var luaTest = new LoadTest();

            //luaTest.Example();

            var program = new Program();

            //program.Sounds.Sounds.PlaySound(0);

            program.Run();
        }


        private void RegisterComponentBuilders()
        {
            BodyComponentBuilder.Register(this);
            AnimationComponentBuilder.Register(this);

            Entities.RegisterComponentBuilder("AngularPositionComponent", AngularPositionComponentBuilder.New);
            Entities.RegisterComponentBuilder("VelocityComponent", VelocityComponentBuilder.New);
            Entities.RegisterComponentBuilder("ThrustComponent", ThrustComponentBuilder.New);
            Entities.RegisterComponentBuilder("PositionComponent", PositionComponentBuilder.New);
            Entities.RegisterComponentBuilder("MotionComponent", MotionComponentBuilder.New);
            Entities.RegisterComponentBuilder("SpriteComponent", SpriteComponentBuilder.New);
            Entities.RegisterComponentBuilder("TextComponent", TextComponentBuilder.New);
            Entities.RegisterComponentBuilder("ClassComponent", ClassComponentBuilder.New);
            Entities.RegisterComponentBuilder("FsmComponent", FsmComponentBuilder.New);
            Entities.RegisterComponentBuilder("TimerComponent", TimerComponentBuilder.New);
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

        private void RegisterEntityTemplates()
        {
            Scripts.RunFile(@"Entities\Actor\Arrow.lua");
            Scripts.RunFile(@"Entities\CrazyMover\CrazyMover.lua");
            Scripts.RunFile(@"Entities\Turret\Turret.lua");
            Scripts.RunFile(@"Entities\Door\DoorHorizontal.lua");
            Scripts.RunFile(@"Entities\Door\DoorVertical.lua");
            Scripts.RunFile(@"Entities\Projectile\Projectile.lua");
            Scripts.RunFile(@"Entities\Teleport\TeleportEntry.lua");
            Scripts.RunFile(@"Entities\Teleport\TeleportExit.lua");
            Scripts.RunFile(@"Entities\WorldGate\WorldGateEntry.lua");
            Scripts.RunFile(@"Entities\WorldGate\WorldGateExit.lua");
        }

        private void RegisterItems()
        {
            Items.Register(new CreditsItem());
        }

        public override void Run()
        {
            window.Run(30.0, 60.0);
        }

        public override void Exit()
        {
            window.Exit();
        }

        #endregion Private Methods
    }
}