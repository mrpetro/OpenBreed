using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class TileGridMan : ITileGridMan
    {
        #region Private Fields

        private readonly ITileMan tileMan;
        private readonly IStampMan stampMan;
        private readonly ILogger logger;
        private readonly TileGrid unknownTileGrid;

        private readonly List<TileGrid> items = new List<TileGrid>();

        #endregion Private Fields

        #region Internal Constructors

        internal TileGridMan(ITileMan tileMan, IStampMan stampMan, ILogger logger)
        {
            this.tileMan = tileMan;
            this.stampMan = stampMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void ModifyTile(int gridId, Vector2 pos, int tileAtlasId, int tileImageId)
        {
            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(gridId, pos, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var grid = GetById(gridId);

            var cellIndex = xIndex + grid.Width * yIndex;
            var tileCell = grid.Cells[cellIndex];
            tileCell.AtlasId = tileAtlasId;
            tileCell.ImageId = tileImageId;
        }

        public void ModifyTiles(int gridId, Vector2 pos, int stampId)
        {
            var stamp = stampMan.GetById(stampId);

            if (stamp == null)
                return;

            int xIndex;
            int yIndex;

            var grid = GetById(gridId);

            if (!TryGetGridIndices(gridId, pos, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            for (int j = 0; j < stamp.Height; j++)
            {
                var cellIndex = xIndex + grid.Width * (yIndex + j);

                for (int i = 0; i < stamp.Width; i++)
                {
                    var stampCell = stamp.Cells[i + stamp.Width * j];
                    grid.Cells[cellIndex].AtlasId = stampCell.AtlasId;
                    grid.Cells[cellIndex].ImageId = stampCell.ImageId;
                    cellIndex++;
                }
            }
        }

        internal TileGrid GetById(int tileGridId)
        {
            return items[tileGridId];
        }

        public void Render(int tileGridId, Box2 clipBox)
        {
            var grid = items[tileGridId];

            var tileSize = grid.CellSize;

            int leftIndex = (int)clipBox.Left / tileSize;
            int bottomIndex = (int)clipBox.Bottom / tileSize;
            int rightIndex = (int)clipBox.Right / tileSize + 1;
            int topIndex = (int)clipBox.Top / tileSize + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, grid.Width);
            rightIndex = MathHelper.Clamp(rightIndex, 0, grid.Width);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, grid.Height);
            topIndex = MathHelper.Clamp(topIndex, 0, grid.Height);

            if (grid.CellBordersVisible)
                DrawCellBorders(grid, leftIndex, bottomIndex, rightIndex, topIndex);

            GL.Enable(EnableCap.Texture2D);

            for (int layerNo = 0; layerNo < grid.LayersNo; layerNo++)
            {
                GL.PushMatrix();
                GL.Translate(leftIndex * tileSize, bottomIndex * tileSize, 0.0f);

                for (int j = bottomIndex; j < topIndex; j++)
                {
                    GL.PushMatrix();

                    for (int i = leftIndex; i < rightIndex; i++)
                    {
                        var index = layerNo * grid.Width * grid.Height + i + grid.Width * j;

                        var cellTile = grid.Cells[index];

                        RenderCellTile(cellTile);

                        GL.Translate(tileSize, 0.0f, 0.0f);
                    }

                    GL.PopMatrix();
                    GL.Translate(0.0f, tileSize, 0.0f);
                }

                GL.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public int CreateGrid(int width, int height, int layersNo, int cellSize)
        {
            var tileGrid = new TileGrid(width, height, layersNo, cellSize);
            return Register(tileGrid);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(TileGrid tileGrid)
        {
            items.Add(tileGrid);

            logger.Verbose($"Tile grid with ID '{items.Count - 1}' created.");

            return items.Count - 1;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Draw debug grid
        /// </summary>
        /// <param name="leftIndex">Left border index of grid</param>
        /// <param name="bottomIndex">Bottom border index of grid</param>
        /// <param name="rightIndex">Right border index of grid</param>
        /// <param name="topIndex">Top border index of grid</param>
        private void DrawCellBorders(TileGrid grid, int leftIndex, int bottomIndex, int rightIndex, int topIndex)
        {
            var cellSize = grid.CellSize;

            GL.Color4(Color4.Green);

            for (int j = bottomIndex; j < topIndex; j++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(leftIndex * cellSize, j * cellSize);
                GL.Vertex2(rightIndex * cellSize, j * cellSize);
                GL.End();
            }

            for (int i = leftIndex; i < rightIndex; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(i * cellSize, bottomIndex * cellSize);
                GL.Vertex2(i * cellSize, topIndex * cellSize);
                GL.End();
            }
        }

        private bool TryGetGridIndices(int gridId, Vector2 point, out int xIndex, out int yIndex)
        {
            var grid = GetById(gridId);

            xIndex = (int)point.X / grid.CellSize;
            yIndex = (int)point.Y / grid.CellSize;
            if (xIndex < 0)
                return false;
            if (yIndex < 0)
                return false;
            if (xIndex >= grid.Width)
                return false;
            if (yIndex >= grid.Height)
                return false;

            return true;
        }

        private void RenderCellTile(TileCell cellTile)
        {
            if (!cellTile.IsEmpty)
            {
                tileMan.Render(cellTile.AtlasId, cellTile.ImageId);
            }
        }

        #endregion Private Methods
    }
}