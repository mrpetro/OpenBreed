using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;

//using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL
{
    public class OpenGLModule : BaseCoreModule, IRenderModule
    {
        #region Private Fields

        private readonly ICoreClient client;

        #endregion Private Fields

        #region Public Constructors

        public OpenGLModule(ICore core, ICoreClient client) : base(core)
        {
            Textures = core.GetManager<ITextureMan>();
            Tiles = core.GetManager<ITileMan>();
            Sprites = core.GetManager<ISpriteMan>();
            Stamps = core.GetManager<IStampMan>();
            Fonts = core.GetManager<IFontMan>();
            this.client = client;
        }

        #endregion Public Constructors

        #region Public Properties

        public ITextureMan Textures { get; }

        public ISpriteMan Sprites { get; }

        public IStampMan Stamps { get; }

        public ITileMan Tiles { get; }

        public IFontMan Fonts { get; }

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

            Core.Events.Raise(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Public Methods

    }
}