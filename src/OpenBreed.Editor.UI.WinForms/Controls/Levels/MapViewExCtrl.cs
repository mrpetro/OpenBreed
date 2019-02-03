using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;

namespace OpenBreed.Editor.UI.WinForms.Controls.Levels
{
    public partial class MapViewExCtrl : UserControl
    {
        int mx = 0;
        int my = 0;


        #region Private Fields

        static float angle = 0.0f;
        private OpenTK.GLControl _glControl;

        private int px;
        private int py;


        private bool _loaded = false;

        #endregion Private Fields

        #region Public Constructors

        public MapViewExCtrl()
        {
            InitializeComponent();

            _glControl = new OpenTK.GLControl();
            _glControl.Dock = DockStyle.Fill;
            Controls.Add(_glControl);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            _glControl.KeyDown += new KeyEventHandler(glControl_KeyDown);
            _glControl.KeyUp += new KeyEventHandler(glControl_KeyUp);
            _glControl.Resize += new EventHandler(glControl_Resize);
            _glControl.MouseMove += _glControl_MouseMove;
            _glControl.MouseLeave += _glControl_MouseLeave;
            _glControl.MouseEnter += _glControl_MouseEnter;
            _glControl.Paint += new PaintEventHandler(glControl_Paint);


            InitGL();



            Application.Idle += Application_Idle;

            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(_glControl, EventArgs.Empty);
        }

        private void _glControl_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void _glControl_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void _glControl_MouseMove(object sender, MouseEventArgs e)
        {
            mx = e.X;
            my = e.Y;
        }

        private void InitGL()
        {
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);                  // Select The Type Of Blending

            GL.Enable(EnableCap.Texture2D);                            // Enable Texture Mapping ( NEW )
                                                                       //glEnable(GL_STENCIL_TEST);
            GL.ClearStencil(0x0);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);				// Black Background
        }

        #endregion Protected Methods

        #region Private Methods

        void Application_Idle(object sender, EventArgs e)
        {
            while (_glControl.IsIdle)
            {
                Render();
            }
        }

        void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyData)
            //{
            //case Keys.Escape:
            //    this.Close();
            //    break;
            //}
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F12)
            //{
            //    GrabScreenshot().Save("screenshot.png");
            //}
        }



        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    Application.Idle -= Application_Idle;

        void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render();

        }

        //    base.OnClosing(e);
        //}
        void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            double aspectRatio = Width / (float)Height;

            GL.Ortho(0, Width, 0, Height, 0, 1); // Origin in lower-left corner
        }
        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PushMatrix();

            var mouseState = OpenTK.Input.Mouse.GetCursorState();
            var mousePos = _glControl.PointToClient(new Point(mouseState.X, mouseState.Y));


            GL.Translate(mousePos.X, Height-mousePos.Y, 0.0);

            GL.Begin(PrimitiveType.Lines);            // These vertices form a closed polygon
            GL.Vertex2(-10.0f, 0.0f);
            GL.Vertex2(10.0f, 0.0f);
            GL.Vertex2(0.0f, -10.0f);
            GL.Vertex2(0.0f, 10.0f);
            GL.End();

            GL.PopMatrix();

            GL.Begin(PrimitiveType.Polygon);            // These vertices form a closed polygon
            GL.Color3(1.0f, 1.0f, 0.0f); // Yellow
            GL.Vertex2(40.0f, 20.0f);
            GL.Vertex2(60.0f, 20.0f);
            GL.Vertex2(70.0f, 40.0f);
            GL.Vertex2(60.0f, 60.0f);
            GL.Vertex2(40.0f, 60.0f);
            GL.Vertex2(30.0f, 40.0f);
            GL.End();

            GL.Flush();

            _glControl.SwapBuffers();
        }

        #endregion Private Methods
    }
}
