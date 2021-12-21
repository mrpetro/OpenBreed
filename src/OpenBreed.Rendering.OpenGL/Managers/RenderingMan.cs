using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private readonly IViewClient viewClient;
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan(IViewClient viewClient, IPrimitiveRenderer primitiveRenderer)
        {
            this.viewClient = viewClient;
            this.primitiveRenderer = primitiveRenderer;
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


        public void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func)
        {
            GL.PushMatrix();

            try
            {
                GL.MultMatrix(ref viewportTransform);

                if (drawBackground)
                    primitiveRenderer.DrawUnitBox(backgroundColor);

                if (drawBorder)
                    primitiveRenderer.DrawUnitRectangle(Color4.Red);

                func.Invoke();
            }
            finally
            {
                GL.PopMatrix();
            }
        }

        #region Private Methods

        private void OnRenderFrame(float dt)
        {
            Fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            Renderable?.Render(Matrix4.Identity, ClipBox, 0, dt);
        }

        private void OnResize(float width, float height)
        {
            GL.LoadIdentity();
            GL.Viewport(0, 0, (int)width, (int)height);
            GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, width, 0.0f, height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);

            ClientResized?.Invoke(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Private Methods
    }
}