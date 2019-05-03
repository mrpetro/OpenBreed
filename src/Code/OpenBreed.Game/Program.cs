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
using OpenBreed.Game.Rendering;
using System.Drawing;
using System.Reflection;

namespace OpenBreed.Game
{
    public class Program : GameWindow
    {
        private string appVersion;
        private Font font;

        public StateMan StateMan { get; }

        public Program()
            : base(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed")
        {
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

            font = new Font("Arial", 12);

            StateMan.OnLoad();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            StateMan.OnResize(ClientRectangle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            StateMan.ProcessInputs(e);
            StateMan.OnUpdate(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"Open Breed (Version: {appVersion} Vsync: {VSync} FPS: {1f / e.Time:0})";

            StateMan.OnRenderFrame(e);

            SwapBuffers();
        }
    }
}
