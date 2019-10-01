using OpenBreed.Core;
using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Actor;
using OpenBreed.Game.Entities.Door;
using OpenBreed.Game.Entities.Projectile;
using OpenBreed.Game.Helpers;
using OpenBreed.Game.States;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Reflection;
using System.Xml;

namespace OpenBreed.Game
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
            Rendering = new OpenGLModule(this);
            Sounds = new OpenALModule(this);
            Physics = new PhysicsModule(this);
            Animations = new AnimationModule(this);
            Blueprints = new BlueprintMan(this);
            Entities = new EntityMan(this);
            Inputs = new InputsMan(this);
            Worlds = new WorldMan(this);
            StateMachine = new StateMan(this);
            MessageBus = new CoreMessageBus(this);
            EventBus = new CoreEventBus(this);
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

        public ILogMan Logging { get; }

        public InputsMan Inputs { get; }

        public CoreMessageBus MessageBus { get; }

        public CoreEventBus EventBus { get; }

        public WorldMan Worlds { get; }

        public StateMan StateMachine { get; }

        public Matrix4 ClientTransform
        {
            get
            {
                var transf = Matrix4.CreateScale(1.0f, -1.0f, 0.0f);
                transf *= Matrix4.CreateTranslation(0.0f, ClientRectangle.Height, 0.0f);
                return transf;
            }
        }

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

            var tileTex = Rendering.Textures.Create("Textures/Tiles/16/Test", @"Content\TileAtlasTest32bit.bmp");
            Rendering.Tiles.Create("Atlases/Tiles/16/Test", tileTex.Id, 16, 4, 4);

            var doorTex = Rendering.Textures.Create("Textures/Sprites/Door", @"Content\DoorSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Door/Horizontal", doorTex.Id, 32, 16, 5, 1, 0, 0);
            Rendering.Sprites.Create("Atlases/Sprites/Door/Vertical", doorTex.Id, 16, 32, 5, 1, 0, 16);

            var laserTex = Rendering.Textures.Create("Textures/Sprites/Laser", @"Content\LaserSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Projectiles/Laser", laserTex.Id, 16, 16, 8, 1, 0, 0);

            var arrowTex = Rendering.Textures.Create("Textures/Sprites/Arrow", @"Content\ArrowSpriteSet.png");
            Rendering.Sprites.Create("Atlases/Sprites/Arrow", arrowTex.Id, 32, 32, 8, 5);

            //Blueprints.Import(@".\Content\BPHorizontalDoor.xml");

            DoorHelper.CreateStamps(this);
            DoorHelper.CreateAnimations(this);
            ActorHelper.CreateAnimations(this);
            ProjectileHelper.CreateAnimations(this);

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

            GL.DepthFunc(DepthFunction.Never);
            //GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, ClientRectangle.Width, 0.0f, ClientRectangle.Height, 0.0f, 1.0f);

            GL.LoadMatrix(ref ortho);
            //GL.Ortho(0, ClientRectangle.Width, 0, ClientRectangle.Height, 0, 1); // Origin in lower-left corner

            Rendering.Viewports.OnClientResize(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            StateMachine.OnResize(ClientRectangle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Inputs.Update();
            PostAndRaise();
            Rendering.Cleanup();
            PostAndRaise();
            Worlds.Cleanup();
            PostAndRaise();
            StateMachine.Update((float)e.Time);
            PostAndRaise();
            Worlds.Update((float)e.Time);
            PostAndRaise();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var fps = 1.0f / (float)e.Time;

            Title = $"Open Breed (Version: {appVersion} Vsync: {VSync} FPS: {fps})";

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
            program.Run(30.0, 60.0);
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