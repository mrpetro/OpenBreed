using OpenBreed.Core.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Events;
using OpenBreed.Core.Modules.Rendering.Managers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering
{
    public class OpenGLModule : BaseCoreModule, IRenderModule
    {
        #region Private Fields

        private readonly SpriteMan spriteMan;
        private readonly StampMan stampMan;
        private readonly FontMan fontMan;
        private float fps;
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

        public float Fps { get { return fps; } }

        #endregion Public Properties

        #region Public Methods

        public void Draw(float dt)
        {
            fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            var clipBox = Box2.FromTLRB(Core.ClientRectangle.Width, 0.0f, Core.ClientRectangle.Height, 0.0f);

            var depth = 0;

            ScreenWorld?.Systems.OfType<ViewportSystem>().FirstOrDefault()?.Render(clipBox, depth, dt);

            DrawCursor();

            GL.PopMatrix();
        }

        public void Cleanup()
        {
        }

        public void OnClientResized(float width, float height)
        {
            Core.Events.Raise(this, GfxEventTypes.CLIENT_RESIZED, new ClientResizedEventArgs(width, height));
        }


        #endregion Public Methods

        #region Private Methods

        private void DrawCursor()
        {
            GL.PushMatrix();

            GL.Translate(Core.Inputs.CursorPos.X, Core.Inputs.CursorPos.Y, 0.0f);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(0, -0.03, 0.0);
            GL.Vertex3(0, 0, 0.0);
            GL.Vertex3(0.015, -0.03, 0.0);
            GL.End();

            GL.PopMatrix();
        }

        #endregion Private Methods
    }
}