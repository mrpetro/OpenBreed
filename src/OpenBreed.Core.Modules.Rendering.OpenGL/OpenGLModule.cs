using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using OpenBreed.Core.Common.Systems;
using OpenTK.Graphics;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering
{
    public class OpenGLModule : IRenderModule
    {
        #region Private Fields

        private readonly SpriteMan spriteMan;
        private readonly FontMan fontMan;
        private TextureMan textureMan;
        private TileMan tileMan;
        private ViewportMan viewportMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenGLModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            viewportMan = new ViewportMan(this);
            textureMan = new TextureMan(this);
            tileMan = new TileMan(this);
            spriteMan = new SpriteMan(this);
            fontMan = new FontMan(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public ITextureMan Textures { get { return textureMan; } }

        public ISpriteMan Sprites { get { return spriteMan; } }

        public ITileMan Tiles { get { return tileMan; } }

        public IFontMan Fonts { get { return fontMan; } }

        public IViewportMan Viewports { get { return viewportMan; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create system for handling sprites
        /// </summary>
        /// <returns>Sprite system</returns>
        public ISpriteSystem CreateSpriteSystem()
        {
            return new SpriteSystem(Core);
        }

        /// <summary>
        /// Create system for handling wireframes
        /// </summary>
        /// <returns>Wireframe system</returns>
        public IWireframeSystem CreateWireframeSystem()
        {
            return new WireframeSystem(Core);
        }

        /// <summary>
        /// Create system for handling texts
        /// </summary>
        /// <returns>Text system</returns>
        public ITextSystem CreateTextSystem()
        {
            return new TextSystem(Core);
        }

        /// <summary>
        /// Create system for handling tiles
        /// </summary>
        /// <returns>Tile system</returns>
        public ITileSystem CreateTileSystem(int gridWidth, int gridHeight, float tileSize, bool drawGrid)
        {
            return new TileSystem(Core, gridWidth, gridHeight, tileSize, drawGrid);
        }

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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            viewportMan.Draw(dt);

            DrawCursor();

            GL.PopMatrix();
        }

        public void Cleanup()
        {
            viewportMan.Cleanup();
        }

        #endregion Public Methods

        #region Private Methods

        private void DrawCursor()
        {
            GL.PushMatrix();

            GL.Translate(Core.Inputs.CursorPos.X, Core.Inputs.CursorPos.Y, 0.0f);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(0, -20, 0.0);
            GL.Vertex3(0, 0, 0.0);
            GL.Vertex3(10, -20, 0.0);
            GL.End();

            GL.PopMatrix();
        }

        public ISprite CreateSprite(string spriteAlias)
        {
            var atlas = Sprites.GetByAlias(spriteAlias);
            return Sprite.Create(atlas.Id);
        }

        public ITile CreateTile(string tileAtlas)
        {
            var atlas = Tiles.GetByAlias(tileAtlas);
            return Tile.Create(atlas.Id);
        }

        #endregion Private Methods
    }
}