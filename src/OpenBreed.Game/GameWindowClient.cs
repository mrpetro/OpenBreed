using OpenBreed.Core;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenBreed.Game
{
    public class GameWindowClient : ICoreClient
    {
        #region Private Fields

        private ICore core;

        private GameWindow window;

        #endregion Private Fields

        #region Public Constructors

        public GameWindowClient(ICore core, int width, int height, string title)
        {
            this.core = core;
            window = new GameWindow(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), title);

            window.MouseDown += (s, a) => core.Inputs.OnMouseDown(a);
            window.MouseUp += (s, a) => core.Inputs.OnMouseUp(a);
            window.MouseMove += (s, a) => core.Inputs.OnMouseMove(a);
            window.MouseWheel += (s, a) => core.Inputs.OnMouseWheel(a);
            window.KeyDown += (s, a) => core.Inputs.OnKeyDown(a);
            window.KeyUp += (s, a) => core.Inputs.OnKeyUp(a);
            window.KeyPress += (s, a) => core.Inputs.OnKeyPress(a);
            window.Load += Window_Load;
            window.Resize += Window_Resize;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 ClientTransform { get; private set; }

        public float ClientRatio { get { return (float)ClientRectangle.Width / (float)ClientRectangle.Height; } }

        public Rectangle ClientRectangle => window.ClientRectangle;

        #endregion Public Properties

        #region Private Methods

        private void Window_Load(object sender, System.EventArgs e)
        {
            core.Load();
        }

        private void Window_Resize(object sender, System.EventArgs e)
        {
            GL.LoadIdentity();
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, ClientRectangle.Width, 0.0f, ClientRectangle.Height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);
            ClientTransform = Matrix4.Identity;
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateTranslation(0.0f, -ClientRectangle.Height, 0.0f));
            ClientTransform = Matrix4.Mult(ClientTransform, Matrix4.CreateScale(1.0f, -1.0f, 1.0f));

            core.Rendering.OnClientResized(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            core.Update((float)e.Time);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            core.Rendering.Draw((float)e.Time);
            window.SwapBuffers();
        }

        public void Exit()
        {
            window.Exit();
        }

        public void Run()
        {
            window.Run(30.0, 60.0);
        }

        #endregion Private Methods
    }
}