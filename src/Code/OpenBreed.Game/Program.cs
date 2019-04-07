using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenBreed.Game.States;

namespace OpenBreed.Game
{
    public class Program : GameWindow
    {
        public StateMan StateMan { get; }

        public Program()
            : base(800, 600, GraphicsMode.Default, "OpenBreed")
        {
            StateMan = new StateMan(this);

            StateMan.RegisterState(new GameState());
            StateMan.RegisterState(new MenuState());
            StateMan.ChangeState(GameState.Id);

            VSync = VSyncMode.On;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run(30.0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            StateMan.ProcessInputs(e);
            StateMan.OnUpdate(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            StateMan.OnRenderFrame(e);

            SwapBuffers();
        }
    }
}
