using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private const int fpsSamplesNo = 60;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IViewClient viewClient;
        private int fpsSampleIndex = 0;
        private float[] fpsSamples = new float[fpsSamplesNo];
        private float fpsSamplesSum = 0;

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

        public float Fps { get; private set; }

        public RenderDelegate Renderer { get; set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox
        { get { return new Box2(0.0f, 0.0f, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y); } }

        #endregion Private Properties

        #region Public Methods

        public void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func)
        {
            primitiveRenderer.PushMatrix();

            try
            {
                primitiveRenderer.MultMatrix(viewportTransform);

                if (drawBackground)
                    primitiveRenderer.DrawUnitRectangle(Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f), backgroundColor, filled: true);

                if (drawBorder)
                    primitiveRenderer.DrawUnitRectangle(Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f), Color4.Red, filled: false);

                func.Invoke();
            }
            finally
            {
                primitiveRenderer.PopMatrix();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private float CalculateMovingAvarage(float newSample)
        {
            fpsSamplesSum -= fpsSamples[fpsSampleIndex];
            fpsSamplesSum += newSample;
            fpsSamples[fpsSampleIndex] = newSample;
            if (++fpsSampleIndex == fpsSamplesNo)
                fpsSampleIndex = 0;

            return fpsSamplesSum / fpsSamplesNo;
        }

        private void OnLoad()
        {
            primitiveRenderer.Load();
        }

        private void OnRenderFrame(float dt)
        {
            Fps = CalculateMovingAvarage(1.0f / dt);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            Renderer?.Invoke(Matrix4.Identity, ClipBox, 0, dt);
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