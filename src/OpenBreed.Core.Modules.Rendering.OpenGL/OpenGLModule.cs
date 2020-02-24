using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
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

        public IEntity ScreenViewport {get; }

        public World ScreenWorld { get; set; }

        public float Fps { get { return fps; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create wireframe render component
        /// </summary>
        /// <param name="thickness">Thickness of wireframe lines</param>
        /// <param name="color">Color of wireframe lines</param>
        /// <returns></returns>
        public IWireframe CreateWireframe(float thickness, Color4 color)
        {
            return new Wireframe(thickness, color);
        }

        public void Draw(float dt)
        {
            fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            GL.Scale(Core.ClientRectangle.Width, Core.ClientRectangle.Height, 1.0f);

            //viewportMan.Draw(dt);

            var viewBox = Box2.FromTLRB(1.0f, 0.0f, 1.0f, 0.0f);
            var depth = 0;

            ScreenWorld?.Systems.OfType<ViewportSystem>().First().Render(viewBox, ref depth, dt);

            DrawCursor();

            GL.PopMatrix();
        }

        public void Cleanup()
        {
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