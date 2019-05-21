using OpenBreed.Core.Systems.Rendering.Components;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Rendering
{
    /// <summary>
    /// System class that is specialized in rendering
    /// </summary>
    public class RenderSystem : WorldSystem<IRenderComponent>, IRenderSystem
    {
        #region Public Fields

        public bool GridVisible = true;
        public int MAX_TILES_COUNT = 1024 * 1024;

        #endregion Public Fields

        #region Private Fields

        private List<ISprite> sprites;
        private Tile[] tiles;
        private List<IDebug> debugs;

        #endregion Private Fields

        #region Public Constructors

        public RenderSystem(int width, int height)
        {
            InitializeTilesMap(width, height);
            sprites = new List<ISprite>();
            debugs = new List<IDebug>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileMapHeight { get; private set; }

        public int TileMapWidth { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///This will draw render system objects into given viewport
        /// </summary>
        /// <param name="viewport">Target viewport to draw render system objects</param>
        public void Draw(Viewport viewport)
        {
            DrawTiles(viewport);

            DrawSprites(viewport);

            DrawDebugs(viewport);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void AddComponent(IRenderComponent component)
        {
            if (component is Tile)
                AddTile((Tile)component);
            else if (component is ISprite)
                AddSprite((ISprite)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IRenderComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddSprite(ISprite sprite)
        {
            sprites.Add(sprite);
        }

        private void AddTile(Tile tile)
        {
            int x, y;
            tile.GetMapIndices(out x, out y);
            var tileId = x + TileMapHeight * y;

            if (x >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            tiles[tileId] = tile;
        }

        /// <summary>
        /// This will draw all debugs to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which debugs will be drawn to</param>
        private void DrawDebugs(Viewport viewport)
        {
            for (int i = 0; i < debugs.Count; i++)
                debugs[i].Draw(viewport);
        }

        /// <summary>
        /// This will draw all sprites to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        private void DrawSprites(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            //GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];
                sprite.Draw(viewport);
            }

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        private void DrawTiles(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            int leftIndex = (int)left / 16;
            int bottomIndex = (int)bottom / 16;
            int rightIndex = (int)right / 16 + 1;
            int topIndex = (int)top / 16 + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, TileMapHeight);
            rightIndex = MathHelper.Clamp(rightIndex, 0, TileMapHeight);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, TileMapWidth);
            topIndex = MathHelper.Clamp(topIndex, 0, TileMapWidth);

            if (GridVisible)
                DrawGrid(leftIndex, bottomIndex, rightIndex, topIndex); 

            GL.Enable(EnableCap.Texture2D);

            for (int j = bottomIndex; j < topIndex; j++)
            {

                for (int i = leftIndex; i < rightIndex; i++)
                {
                    var tile = tiles[i + TileMapHeight * j];

                    if (tile != null)
                        tile.Draw(viewport);
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        /// <summary>
        /// Draw debug grid
        /// </summary>
        /// <param name="leftIndex">Left border index of grid</param>
        /// <param name="bottomIndex">Bottom border index of grid</param>
        /// <param name="rightIndex">Right border index of grid</param>
        /// <param name="topIndex">Top border index of grid</param>
        private void DrawGrid(int leftIndex, int bottomIndex, int rightIndex, int topIndex)
        {
            GL.Color4(Color4.Green);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(leftIndex * 16, j * 16);
                GL.Vertex2(rightIndex * 16, j * 16);
                GL.End();
            }

            for (int i = leftIndex; i < rightIndex; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(i * 16, bottomIndex * 16);
                GL.Vertex2(i * 16, topIndex * 16);
                GL.End();
            }
        }

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            tiles = new Tile[width * height];
        }

        #endregion Private Methods
    }
}