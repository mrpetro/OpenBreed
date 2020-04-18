using OpenBreed.Core.Common;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Managers;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering
{
    public class OpenGLModule : BaseCoreModule, IRenderModule
    {
        #region Private Fields

        private readonly SpriteMan spriteMan;
        private readonly StampMan stampMan;
        private readonly FontMan fontMan;
        private TextureMan textureMan;
        private TileMan tileMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenGLModule(ICore core) : base(core)
        {
            textureMan = new TextureMan(this);
            tileMan = new TileMan(this);
            spriteMan = new SpriteMan(this);
            stampMan = new StampMan(this);
            fontMan = new FontMan(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ITextureMan Textures { get { return textureMan; } }

        public ISpriteMan Sprites { get { return spriteMan; } }

        public IStampMan Stamps { get { return stampMan; } }

        public ITileMan Tiles { get { return tileMan; } }

        public IFontMan Fonts { get { return fontMan; } }

        public World ScreenWorld { get; set; }

        public float Fps { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox { get { return Box2.FromTLRB(Core.ClientRectangle.Width, 0.0f, Core.ClientRectangle.Height, 0.0f); } }

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
            Core.Events.Raise(this, new ClientResizedEventArgs(width, height));
        }

        #endregion Public Methods
    }
}