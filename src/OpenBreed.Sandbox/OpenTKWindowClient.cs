using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;

namespace OpenBreed.Sandbox
{
    public class OpenTKWindowClient : IViewClient
    {
        #region Private Fields

        private readonly GameWindow window;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKWindowClient(int width, int height, string title)
        {
            var gameWindowSettings = new GameWindowSettings()
            {
                UpdateFrequency = 30
            };

            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(width, height),
                RedBits = 8,
                GreenBits = 8,
                BlueBits = 8,
                AlphaBits = 8,
                DepthBits = 24,
                StencilBits = 8,
                Title = title,
                Flags = ContextFlags.ForwardCompatible,
            };

            window = new GameWindow(gameWindowSettings, nativeWindowSettings);

            //window = new GameWindow(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), title);

            window.Load += Window_Load;
            window.Resize += Window_Resize;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
            window.VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Events

        public event Action<MouseButtonEventArgs> MouseDownEvent
        {
            add => window.MouseDown += value;
            remove => window.MouseDown -= value;
        }

        public event Action<MouseButtonEventArgs> MouseUpEvent
        {
            add => window.MouseUp += value;
            remove => window.MouseUp -= value;
        }

        public event Action<MouseMoveEventArgs> MouseMoveEvent
        {
            add => window.MouseMove += value;
            remove => window.MouseMove -= value;
        }

        public event Action<MouseWheelEventArgs> MouseWheelEvent
        {
            add => window.MouseWheel += value;
            remove => window.MouseWheel -= value;
        }

        public event Action<KeyboardKeyEventArgs> KeyDownEvent
        {
            add => window.KeyDown += value;
            remove => window.KeyDown -= value;
        }

        public event Action<KeyboardKeyEventArgs> KeyUpEvent
        {
            add => window.KeyUp += value;
            remove => window.KeyUp -= value;
        }

        //public event EventHandler<KeyPressEventArgs> KeyPressEvent
        //{
        //    add => window.KeyPress += value;
        //    remove => window.KeyPress -= value;
        //}

        public event Action<float> UpdateFrameEvent;

        public event Action<float> RenderFrameEvent;

        public event Action LoadEvent;

        public event Action<Vector2i> ResizeEvent;

        #endregion Public Events

        #region Public Properties

        public Matrix4 ClientTransform { get; private set; }

        public float ClientRatio { get { return (float)ClientRectangle.Size.X / (float)ClientRectangle.Size.Y; } }

        public Box2i ClientRectangle => window.ClientRectangle;

        public KeyboardState KeyboardState => window.KeyboardState;

        public MouseState MouseState => window.MouseState;

        #endregion Public Properties

        #region Public Methods

        public void Exit()
        {
            window.Close();
        }

        public void Run()
        {
            window.Run();
        }

        #endregion Public Methods

        #region Private Methods

        private void Window_Load()
        {
            LoadEvent?.Invoke();
        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Size.Y, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(-ClientRectangle.Size.X / 2.0f, ClientRectangle.Size.Y / 2.0f, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(2.0f / ClientRectangle.Size.X, -2.0f / ClientRectangle.Size.Y, 1.0f));

            ResizeEvent?.Invoke(new Vector2i(ClientRectangle.Size.X, ClientRectangle.Size.Y));
        }

        private void Window_UpdateFrame(FrameEventArgs e)
        {
            UpdateFrameEvent?.Invoke((float)e.Time);
        }

        private void Window_RenderFrame(FrameEventArgs e)
        {
            RenderFrameEvent.Invoke((float)e.Time);

            window.SwapBuffers();
        }

        #endregion Private Methods
    }
}