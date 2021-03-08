using OpenBreed.Common;
using OpenBreed.Core;

//using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        private readonly IViewClient viewClient;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan(IEventsMan eventsMan, IViewClient viewClient, IWorldMan worldMan)
        {
            this.eventsMan = eventsMan;
            this.viewClient = viewClient;
            this.worldMan = worldMan;

            viewClient.ResizeEvent += (s, a) => OnResize(a.Width, a.Height);
            viewClient.RenderFrameEvent += (s, a) => OnRenderFrame(a);
        }

        #endregion Public Constructors

        #region Public Properties

        public World ScreenWorld { get; set; }

        public float Fps { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox { get { return Box2.FromTLRB(viewClient.ClientRectangle.Width, 0.0f, viewClient.ClientRectangle.Height, 0.0f); } }

        #endregion Private Properties

        #region Public Methods

        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            eventsMan.Subscribe(this, callback);
        }

        public void Cleanup()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnRenderFrame(float dt)
        {
            Fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            try
            {
                GL.PushMatrix();

                ScreenWorld?.Systems.OfType<IRenderableSystem>().ForEach(item => item.Render(ClipBox, 0, dt));
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

            eventsMan.Raise(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Private Methods
    }
}