using OpenBreed.Core;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Animation;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Movement;
using OpenBreed.Core.Systems.Physics;
using OpenBreed.Core.Systems.Rendering;
using OpenBreed.Core.Systems.Sound;
using OpenBreed.Game.States;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace OpenBreed.Game
{
    public class Program : GameWindow, ICore
    {
        #region Private Fields

        private string appVersion;
        private Font font;

        #endregion Private Fields

        #region Public Constructors

        public Program()
            : base(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed")
        {
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Entities = new EntityMan();
            Worlds = new WorldMan(this);
            States = new StateMan(this);
            Viewports = new ViewportMan(this);
            States.RegisterState(new StateTechDemo1(this));
            States.RegisterState(new StateTechDemo2(this));
            //StateMan.RegisterState(new MenuState(this));
            States.SetNextState(StateTechDemo1.Id);
            States.ChangeState();

            VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Properties

        public EntityMan Entities { get; }
        public WorldMan Worlds { get; }
        public ViewportMan Viewports { get; }
        public StateMan States { get; }

        #endregion Public Properties

        #region Public Methods

        public ISoundSystem CreateSoundSystem()
        {
            return new SoundSystem();
        }

        public IMovementSystem CreateMovementSystem()
        {
            return new MovementSystem();
        }

        public IAnimationSystem CreateAnimationSystem()
        {
            return new AnimationSystem();
        }

        public IControlSystem CreateControlSystem()
        {
            return new ControlSystem();
        }

        public IPhysicsSystem CreatePhysicsSystem()
        {
            return new PhysicsSystem(64, 64);
        }

        public IRenderSystem CreateRenderSystem()
        {
            return new RenderSystem(64, 64);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            font = new Font("Arial", 12);

            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);                  // Select The Type Of Blending

            //GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0x0);
            GL.StencilMask(0xFFFFFFFF);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Ortho(0, ClientRectangle.Width, 0, ClientRectangle.Height, 0, 1); // Origin in lower-left corner

            States.OnResize(ClientRectangle);
        }

        protected void OnProcessInputs(float dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Number1))
                States.SetNextState(StateTechDemo1.Id);
            else if (keyState.IsKeyDown(Key.Number2))
                States.SetNextState(StateTechDemo2.Id);

            Worlds.ProcessInputs(dt);
        }

        protected void OnUpdate(float dt)
        {
            Worlds.Update(dt);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            OnProcessInputs((float)e.Time);

            States.ProcessInputs(e);

            OnUpdate((float)e.Time);
            States.OnUpdate(e);

            Worlds.Cleanup();
            Viewports.Cleanup();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"Open Breed (Version: {appVersion} Vsync: {VSync} FPS: {1f / e.Time:0})";

            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            Viewports.Draw((float)e.Time);

            GL.PopMatrix();

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
            program.Run(30.0);
        }

        #endregion Private Methods
    }
}