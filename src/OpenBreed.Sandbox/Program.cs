using OpenBreed.Core;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Shapes;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Audio.Builders;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Systems.Control.Systems;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Button;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Items;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBreed.Sandbox
{
    public class Program : GameWindow, ICore
    {
        #region Private Fields

        private string appVersion;

        private Dictionary<Type, ICoreModule> modules = new Dictionary<Type, ICoreModule>();

        #endregion Private Fields

        #region Public Constructors

        public Program()
            : base(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed")
        {
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Logging = new LogMan(this);

            Scripts = new LuaScriptMan(this);
            Commands = new CommandsMan(this);
            Events = new EventsMan(this);
            Entities = new EntityMan(this);
            StateMachines = new FsmMan(this);
            Shapes = new ShapeMan(this);
            Players = new PlayersMan(this);
            Items = new ItemsMan(this);
            Inputs = new InputsMan(this);
            Worlds = new WorldMan(this);
            Jobs = new JobMan(this);

            Rendering = new OpenGLModule(this);
            Physics = new PhysicsModule(this);
            Sounds = new OpenALModule(this);
            Animations = new AnimMan(this);

            RegisterModule(Rendering);
            RegisterModule(Physics);
            RegisterModule(Sounds);

            VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderModule Rendering { get; }

        public PhysicsModule Physics { get; }

        public IAudioModule Sounds { get; }

        public AnimMan Animations { get; }

        public EntityMan Entities { get; }

        public FsmMan StateMachines { get; }

        public ShapeMan Shapes { get; }

        public PlayersMan Players { get; }

        public ILogMan Logging { get; }

        public JobMan Jobs { get; }

        public ItemsMan Items { get; }

        public InputsMan Inputs { get; }

        public CommandsMan Commands { get; }

        public EventsMan Events { get; }

        public WorldMan Worlds { get; }

        public IScriptMan Scripts { get; }

        public Matrix4 ClientTransform { get; private set; }

        public float ClientRatio { get { return (float)ClientRectangle.Width / (float)ClientRectangle.Height; } }

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

        public T GetModule<T>() where T : ICoreModule
        {
            return (T)modules[typeof(T)];
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Inputs.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Inputs.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            Inputs.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Inputs.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            Inputs.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            Inputs.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            Inputs.OnKeyPress(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = $"Open Breed Sandbox (Version: {appVersion} Vsync: {VSync})";

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
            Rendering.Sprites.Create("Atlases/Sprites/Arrow", arrowTex.Id, 32, 32, 8, 5);

            var cursorsTex = Rendering.Textures.Create("Textures/Sprites/Cursors", @"Content\Graphics\Cursors.png");
            Rendering.Sprites.Create("Atlases/Sprites/Cursors", cursorsTex.Id, 16, 16, 1, 1);

            CameraHelper.CreateAnimations(this);
            DoorHelper.CreateStamps(this);
            //PickableHelper.CreateStamps(this);
            DoorHelper.CreateAnimations(this);
            ActorHelper.CreateAnimations(this);
            TeleportHelper.CreateAnimations(this);
            ProjectileHelper.CreateAnimations(this);

            DoorHelper.CreateHorizontalFSM(this);
            DoorHelper.CreateVerticalFSM(this);
            ProjectileHelper.CreateFsm(this);
            ButtonHelper.CreateFSM(this);
            ActorHelper.CreateAttackingFSM(this);

            Rendering.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            HudWorldHelper.CreateHudWorld(this);

            GameWorldHelper.CreateGameWorld(this);

            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);                  // Select The Type Of Blending

            //GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0x0);
            GL.StencilMask(0xFFFFFFFF);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);

            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.DepthTest);

            //var func = Scripts.RunFile(@"Entities\Actor\States\Attacking\IdleState.lua");

            //var r = func.Invoke();

            //var result = Scripts.RunFile("Content\\Scripts\\start.lua");

            OnEngineInitialized();

            //var myass = (IViewport)Scripts.GetObject("myass");
        }

        protected void OnEngineInitialized()
        {
            Scripts.TryInvokeFunction("EngineInitialized");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, ClientRectangle.Width, 0.0f, ClientRectangle.Height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Height, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(1.0f,-1.0f,1.0f));
            Rendering.OnClientResized(ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Worlds.Cleanup();
            Rendering.Cleanup();
            Players.ResetInputs();
            Inputs.Update();
            Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            Worlds.Update((float)e.Time);
            Jobs.Update((float)e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            Rendering.Draw((float)e.Time);

            SwapBuffers();
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

            program.Run(30.0, 60.0);
        }

        private void RegisterModule(ICoreModule module)
        {
            modules.Add(module.GetType(), module);
        }

        private void RegisterComponentBuilders()
        {
            BodyComponentBuilder.Register(this);
            AnimationComponentBuilder.Register(this);
            DirectionComponentBuilder.Register(this);

            Entities.RegisterComponentBuilder("VelocityComponent", VelocityComponentBuilder.New);
            Entities.RegisterComponentBuilder("ThrustComponent", ThrustComponentBuilder.New);
            Entities.RegisterComponentBuilder("PositionComponent", PositionComponentBuilder.New);
            Entities.RegisterComponentBuilder("MotionComponent", MotionComponentBuilder.New);
            Entities.RegisterComponentBuilder("SpriteComponent", SpriteComponentBuilder.New);
            Entities.RegisterComponentBuilder("TextComponent", TextComponentBuilder.New);
        }

        private void RegisterShapes()
        {
            Shapes.Register("Shapes/Box_0_0_16_16", new BoxShape(0, 0, 16, 16));
            Shapes.Register("Shapes/Box_16_16_8_8", new BoxShape(16, 16, 8, 8));
            Shapes.Register("Shapes/Box_0_0_16_32", new BoxShape(0, 0, 16, 32));
            Shapes.Register("Shapes/Box_0_0_32_16", new BoxShape(0, 0, 32, 16));
            Shapes.Register("Shapes/Box_0_0_32_32", new BoxShape(0, 0, 32, 32));
        }

        private void RegisterFixtures()
        {
            Physics.Fixturs.Create("Fixtures/GridCell", "Trigger", Shapes.GetByTag("Shapes/Box_0_0_16_16"));
            Physics.Fixturs.Create("Fixtures/TeleportEntry", "Trigger", Shapes.GetByTag("Shapes/Box_16_16_8_8"));
            Physics.Fixturs.Create("Fixtures/TeleportExit", "Trigger", Shapes.GetByTag("Shapes/Box_16_16_8_8"));
            Physics.Fixturs.Create("Fixtures/Projectile", "Dynamic", Shapes.GetByTag("Shapes/Box_0_0_16_16"));
            Physics.Fixturs.Create("Fixtures/DoorVertical", "Static", Shapes.GetByTag("Shapes/Box_0_0_16_32"));
            Physics.Fixturs.Create("Fixtures/DoorHorizontal", "Static", Shapes.GetByTag("Shapes/Box_0_0_32_16"));
            Physics.Fixturs.Create("Fixtures/Arrow", "Dynamic", Shapes.GetByTag("Shapes/Box_0_0_32_32"));
        }

        private void RegisterEntityTemplates()
        {
            Scripts.RunFile(@"Entities\Actor\Arrow.lua");
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

        #endregion Private Methods
    }
}