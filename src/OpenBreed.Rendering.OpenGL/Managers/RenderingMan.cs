using OpenBreed.Core;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan(IViewClient viewClient)
        {
            this.viewClient = viewClient;

            viewClient.ResizeEvent += (s, a) => OnResize(a.Width, a.Height);
            viewClient.RenderFrameEvent += (s, a) => OnRenderFrame(a);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<ClientResizedEventArgs> ClientResized;

        #endregion Public Events

        #region Public Properties

        public IRenderableBatch Renderable { get; set; }

        public float Fps { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox
        { get { return Box2.FromTLRB(viewClient.ClientRectangle.Width, 0.0f, viewClient.ClientRectangle.Height, 0.0f); } }

        #endregion Private Properties

        #region Private Methods

        private void OnRenderFrame(float dt)
        {
            Fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            try
            {
                GL.PushMatrix();

                Renderable?.Render(ClipBox, 0, dt);
            }
            finally
            {
                GL.PopMatrix();
            }
        }

        private void OnResize(float width, float height)
        {
            GL.LoadIdentity();
            GL.Viewport(0, 0, (int)width, (int)height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, width, 0.0f, height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);

            ClientResized?.Invoke(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Private Methods
    }
}