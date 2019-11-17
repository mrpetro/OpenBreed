using OpenBreed.Core;
using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Inputs;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Systems;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Items;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.States;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Reflection;
using System.Xml;

namespace OpenBreed.Sandbox
{
    public class Program : GameWindow, ICore
    {
        #region Private Fields

        private string appVersion;

        #endregion Private Fields

        #region Public Constructors

        public Program()
            : base(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed")
        {
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Logging = new LogMan(this);
            MessageBus = new CoreMessageBus(this);
            EventBus = new CoreEventBus(this);
            Rendering = new OpenGLModule(this);
            Sounds = new OpenALModule(this);
            Physics = new PhysicsModule(this);
            Animations = new AnimationModule(this);
            Blueprints = new BlueprintMan(this);
            Entities = new EntityMan(this);
            Players = new PlayersMan(this);
            Items = new ItemsMan(this);
            Inputs = new InputsMan(this);
            Worlds = new WorldMan(this);
            StateMachine = new StateMan(this);

            Jobs = new JobMan(this);
            VSync = VSyncMode.On;

            RegisterBlueprintParsers();
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderModule Rendering { get; }

        public IAudioModule Sounds { get; }

        public IPhysicsModule Physics { get; }

        public IAnimationModule Animations { get; }

        public BlueprintMan Blueprints { get; }

        public EntityMan Entities { get; }

        public PlayersMan Players { get; }

        public ILogMan Logging { get; }

        public JobMan Jobs { get; }

        public ItemsMan Items { get; }

        public InputsMan Inputs { get; }

        public CoreMessageBus MessageBus { get; }

        public CoreEventBus EventBus { get; }

        public WorldMan Worlds { get; }

        public StateMan StateMachine { get; }

        private HudWorld hudWorld;

        public Matrix4 ClientTransform
        {
            get
            {
                var transf = Matrix4.CreateScale(ClientRectangle.Width, -ClientRectangle.Height, 1.0f);
                transf.Invert();
                transf = Matrix4.Mult(transf, Matrix4.CreateTranslation(0.0f, 1.0f, 0.0f));
                return transf;
            }
        }

        public float ClientRatio { get { return (float)ClientRectangle.Width / (float)ClientRectangle.Height; } }

        #endregion Public Properties

        #region Public Methods

        public GroupSystem CreateGroupSystem()
        {
            return new GroupSystem(this);
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

            var laserTex = Rendering.Textures.Create("Textures/Sprites/Laser", @"Content\Graphics\LaserSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var arrowTex = Rendering.Textures.Create("Textures/Sprites/Arrow", @"Content\Graphics\ArrowSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Arrow", arrowTex.Id, 32, 32, 8, 5);

            //Blueprints.Import(@".\Content\BPHorizontalDoor.xml");

            CameraHelper.CreateAnimations(this);
            DoorHelper.CreateStamps(this);
            PickableHelper.CreateStamps(this);
            DoorHelper.CreateAnimations(this);
            ActorHelper.CreateAnimations(this);
            TeleportHelper.CreateAnimations(this);
            ProjectileHelper.CreateAnimations(this);

            hudWorld = new HudWorld(this);
            hudWorld.Setup();

            StateMachine.RegisterState(new StateTechDemo1(this));
            StateMachine.RegisterState(new StateTechDemo2(this));
            StateMachine.RegisterState(new StateTechDemo3(this));
            StateMachine.RegisterState(new StateTechDemo4(this));
            StateMachine.RegisterState(new StateTechDemo5(this));
            StateMachine.RegisterState(new StateTechDemo6(this));
            //StateMan.RegisterState(new MenuState(this));
            StateMachine.SetNextState(StateTechDemo6.ID);
            StateMachine.ChangeState();

            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);                  // Select The Type Of Blending

            //GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0x0);
            GL.StencilMask(0xFFFFFFFF);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);

            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, ClientRectangle.Width, 0.0f, ClientRectangle.Height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);

            StateMachine.OnResize(ClientRectangle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);


            Players.ResetInputs();
            Inputs.Update();
            Players.ApplyInputs();
            PostAndRaise();
            Rendering.Cleanup();
            PostAndRaise();
            Worlds.Cleanup();
            PostAndRaise();
            StateMachine.Update((float)e.Time);
            PostAndRaise();
            Jobs.Update((float)e.Time);
            PostAndRaise();
            Worlds.Update((float)e.Time);
            PostAndRaise();

            hudWorld.Update();

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

        private void RegisterItems()
        {
            Items.Register(new CreditsItem());
        }

        private void RegisterBlueprintParsers()
        {
            ComponentStateXml.RegisterTypeParser(typeof(Vector2), ReadVector2);
            ComponentStateXml.RegisterTypeParser(typeof(string), ReadString);

            //EntityMan.RegisterBuilder("OpenBreed.Core.Common.Components.GridPosition", GridPositionBuilder.Create);
        }

        private object ReadString(XmlReader reader)
        {
            reader.ReadStartElement();
            var text = reader.ReadContentAsString();
            reader.ReadEndElement();
            return text;
        }

        private object ReadVector2(XmlReader reader)
        {
            reader.ReadStartElement();

            if (reader.Name != "X")
                throw new FormatException("Expected 'X' value");

            reader.ReadStartElement();
            var x = reader.ReadContentAsFloat();
            reader.ReadEndElement();

            if (reader.Name != "Y")
                throw new FormatException("Expected 'Y' value");

            reader.ReadStartElement();
            var y = reader.ReadContentAsFloat();
            reader.ReadEndElement();

            reader.ReadEndElement();
            return new Vector2(x, y);
        }

        private void PostAndRaise()
        {
            MessageBus.PostEnqueued();
            EventBus.RaiseEnqueued();
        }

        #endregion Private Methods
    }
}