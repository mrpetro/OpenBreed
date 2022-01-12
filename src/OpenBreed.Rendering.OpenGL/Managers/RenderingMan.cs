using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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
            viewClient.ResizeEvent += (a) => OnResize(a.X, a.Y);
            viewClient.LoadEvent += () => OnLoad();
            viewClient.RenderFrameEvent += (a) => OnRenderFrame(a);
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
        { get { return new Box2(0.0f, 0.0f, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y); } }

        #endregion Private Properties


        public void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func)
        {
            primitiveRenderer.PushMatrix();

            try
            {
                primitiveRenderer.MultMatrix(viewportTransform);

                if (drawBackground)
                    primitiveRenderer.DrawUnitBox(Matrix4.Identity, backgroundColor);

                if (drawBorder)
                    primitiveRenderer.DrawUnitRectangle(Matrix4.Identity, Color4.Red);

                func.Invoke();
            }
            finally
            {
                primitiveRenderer.PopMatrix();
            }
        }

        #region Private Methods

        private void OnLoad()
        {
            primitiveRenderer.Load();
        }

        private void OnRenderFrame(float dt)
        {
            Fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);


            Renderable?.Render(Matrix4.Identity, ClipBox, 0, dt);
        }

        private void OnResize(int width, int height)
        {
            primitiveRenderer.SetProjection(Matrix4.Identity);
            GL.Viewport(0, 0, width, height);

            primitiveRenderer.SetView(Matrix4.CreateOrthographicOffCenter(0.0f, width, 0.0f, height, -100.0f, 100.0f));

            ClientResized?.Invoke(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Private Methods
    }
}