using OpenBreed.Common;
using OpenBreed.Core;

//using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
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

        private readonly IClientMan client;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan(IEventsMan eventsMan, IClientMan client, IWorldMan worldMan)
        {
            this.eventsMan = eventsMan;
            this.client = client;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public World ScreenWorld { get; set; }

        public float Fps { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox { get { return Box2.FromTLRB(client.ClientRectangle.Width, 0.0f, client.ClientRectangle.Height, 0.0f); } }

        #endregion Private Properties

        #region Public Methods

        public void Draw(float dt)
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

        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            eventsMan.Subscribe(this, callback);
        }

        public void Cleanup()
        {
        }

        public void OnClientResized(float width, float height)
        {
            GL.LoadIdentity();
            GL.Viewport(0, 0, (int)width, (int)height);
            GL.MatrixMode(MatrixMode.Modelview);
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, width, 0.0f, height, -100.0f, 100.0f);
            GL.LoadMatrix(ref ortho);

            eventsMan.Raise(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Public Methods
    }
}