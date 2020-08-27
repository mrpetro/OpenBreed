using OpenBreed.Core;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Drawing;

namespace OpenBreed.Sandbox
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
        }

        private void Window_Load(object sender, EventArgs e)
        {
            //window.Title = $"Open Breed Sandbox (Version: {appVersion} Vsync: {window.VSync})";
        }

        #endregion Public Constructors

        #region Public Properties

        public Matrix4 ClientTransform => throw new NotImplementedException();

        public float ClientRatio => throw new NotImplementedException();

        public Rectangle ClientRectangle => throw new NotImplementedException();

        #endregion Public Properties
    }
}