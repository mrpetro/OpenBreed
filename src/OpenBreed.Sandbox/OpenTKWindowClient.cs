using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
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
            window = new GameWindow(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), title);

            window.Load += Window_Load;
            window.Resize += Window_Resize;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
            window.VSync = VSyncMode.On;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<MouseButtonEventArgs> MouseDownEvent
        {
            add => window.MouseDown += value;
            remove => window.MouseDown -= value;
        }

        public event EventHandler<MouseButtonEventArgs> MouseUpEvent
        {
            add => window.MouseUp += value;
            remove => window.MouseUp -= value;
        }

        public event EventHandler<MouseMoveEventArgs> MouseMoveEvent
        {
            add => window.MouseMove += value;
            remove => window.MouseMove -= value;
        }

        public event EventHandler<MouseWheelEventArgs> MouseWheelEvent
        {
            add => window.MouseWheel += value;
            remove => window.MouseWheel -= value;
        }

        public event EventHandler<KeyboardKeyEventArgs> KeyDownEvent
        {
            add => window.KeyDown += value;
            remove => window.KeyDown -= value;
        }

        public event EventHandler<KeyboardKeyEventArgs> KeyUpEvent
        {
            add => window.KeyUp += value;
            remove => window.KeyUp -= value;
        }

        public event EventHandler<KeyPressEventArgs> KeyPressEvent
        {
            add => window.KeyPress += value;
            remove => window.KeyPress -= value;
        }

        public event EventHandler<float> UpdateFrameEvent;

        public event EventHandler<float> RenderFrameEvent;

        public event EventHandler LoadEvent;

        public event EventHandler<Vector2> ResizeEvent;

        #endregion Public Events

        #region Public Properties

        public Matrix4 ClientTransform { get; private set; }

        public float ClientRatio { get { return (float)ClientRectangle.Width / (float)ClientRectangle.Height; } }

        public Rectangle ClientRectangle => window.ClientRectangle;

        #endregion Public Properties

        #region Public Methods

        public void Exit()
        {
            window.Exit();
        }

        public void Run()
        {
            window.Run(30.0, 60.0);
        }

        #endregion Public Methods

        #region Private Methods

        private void Window_Load(object sender, System.EventArgs e)
        {
            LoadEvent?.Invoke(sender, new EventArgs());
        }

        private void Window_Resize(object sender, System.EventArgs e)
        {
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Height, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(1.0f, -1.0f, 1.0f));

            ResizeEvent?.Invoke(sender, new Vector2(ClientRectangle.Width, ClientRectangle.Height));
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            UpdateFrameEvent?.Invoke(sender, (float)e.Time);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            RenderFrameEvent.Invoke(sender, (float)e.Time);

            window.SwapBuffers();
        }

        #endregion Private Methods
    }
}