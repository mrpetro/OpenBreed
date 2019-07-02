﻿using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace OpenBreed.Core.Modules.Rendering
{
    public class OpenGLModule : IRenderModule
    {
        #region Private Fields

        private TextureMan textureMan = new TextureMan();
        private TileMan tileMan = new TileMan();
        private readonly SpriteMan spriteMan;
        private readonly FontMan fontMan;
        private ViewportMan viewportMan = new ViewportMan();

        #endregion Private Fields

        #region Public Constructors

        public OpenGLModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

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
        public IWorldSystem CreateSpriteSystem()
        {
            return new SpriteSystem(Core);
        }

        /// <summary>
        /// Create system for handling texts
        /// </summary>
        /// <returns>Text system</returns>
        public IWorldSystem CreateTextSystem()
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
        /// Creates text component using given font
        /// </summary>
        /// <param name="fontId">Id of font to use for this text component</param>
        /// <param name="offset">Offset position from position component</param>
        /// <param name="value">Optional initial text value</param>
        /// <returns>Text component</returns>
        public IText CreateText(int fontId, Vector2 offset, string value = null)
        {
            return new Text(fontId, offset, value);
        }

        public ISprite CreateSprite(int atlasId, int imageId = 0)
        {
            return new Sprite(atlasId, imageId);
        }

        public ITile CreateTile(int atlasId, int imageId = 0)
        {
            return new Tile(atlasId, imageId);
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

        #endregion Private Methods
    }
}