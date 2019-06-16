using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TileSystem
    {
        #region Public Fields

        /// <summary>
        /// Debug flag
        /// </summary>
        public bool GridVisible = true;

        #endregion Public Fields

        #region Private Fields

        private ITile[] tiles;

        #endregion Private Fields

        #region Public Constructors

        public TileSystem(ICore core, int width, int height)
        {
            InitializeTilesMap(width, height);
        }

        #endregion Public Constructors

        #region Public Properties

        public int TileMapHeight { get; private set; }

        public int TileMapWidth { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Draw(Viewport viewport)
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

        public void AddTile(ITile tile)
        {
            int x, y;
            GetMapIndices(tile, tile.Position, out x, out y);
            var tileId = x + TileMapHeight * y;

            if (x >= TileMapWidth)
                throw new InvalidOperationException($"Tile X coordinate exceeds tile map width size.");

            if (y >= TileMapHeight)
                throw new InvalidOperationException($"Tile Y coordinate exceeds tile map height size.");

            tiles[tileId] = tile;
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeTilesMap(int width, int height)
        {
            TileMapHeight = width;
            TileMapWidth = height;
            tiles = new Tile[width * height];
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

        private void GetMapIndices(ITile tile, Position position, out int x, out int y)
        {
            x = (int)(position.Value.X / tile.Size);
            y = (int)(position.Value.Y / tile.Size);
        }

        #endregion Private Methods
    }
}