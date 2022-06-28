using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class TileGrid : ITileGrid
    {
        #region Private Fields

        private readonly ITileMan tileMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public TileGrid(ITileMan tileMan, IPrimitiveRenderer primitiveRenderer, IStampMan stampMan, int width, int height, int layersNo, int cellSize)
        {
            this.tileMan = tileMan;
            this.primitiveRenderer = primitiveRenderer;
            this.stampMan = stampMan;
            Width = width;
            Height = height;
            LayersNo = layersNo;
            Cells = CreateTileArray();
            CellSize = cellSize;
            CellBordersVisible = false;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Width { get; }

        public int Height { get; }

        public int LayersNo { get; }

        public int CellSize { get; }

        public bool CellBordersVisible { get; }

        public TileCell[] Cells { get; }

        #endregion Public Properties

        #region Public Methods

        public void ModifyTile(Vector2i pos, int tileAtlasId, int tileImageId)
        {
            if(!ValidateGridIndices(pos.X, pos.Y))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = pos.X + Width * pos.Y;
            var tileCell = Cells[cellIndex];
            tileCell.AtlasId = tileAtlasId;
            tileCell.ImageId = tileImageId;
        }

        public void ModifyTile(Vector2 pos, int tileAtlasId, int tileImageId)
        {
            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(pos, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            var cellIndex = xIndex + Width * yIndex;
            var tileCell = Cells[cellIndex];
            tileCell.AtlasId = tileAtlasId;
            tileCell.ImageId = tileImageId;
        }

        public void ModifyTiles(Vector2 pos, int stampId)
        {
            var stamp = stampMan.GetById(stampId);

            if (stamp == null)
                return;

            int xIndex;
            int yIndex;

            if (!TryGetGridIndices(pos, out xIndex, out yIndex))
                throw new InvalidOperationException($"Tile position exceeds tile grid limits.");

            for (int j = 0; j < stamp.Height; j++)
            {
                var cellIndex = xIndex - stamp.OriginX + Width * (yIndex + j - stamp.OriginY);

                for (int i = 0; i < stamp.Width; i++)
                {
                    var stampCell = stamp.Cells[i + stamp.Width * j];
                    Cells[cellIndex].AtlasId = stampCell.AtlasId;
                    Cells[cellIndex].ImageId = stampCell.ImageId;
                    cellIndex++;
                }
            }
        }

        public void Render(Box2 clipBox)
        {
            int leftIndex = (int)clipBox.Min.X / CellSize;
            int bottomIndex = (int)clipBox.Min.Y / CellSize;
            int rightIndex = (int)clipBox.Max.X / CellSize + 1;
            int topIndex = (int)clipBox.Max.Y / CellSize + 1;

            leftIndex = MathHelper.Clamp(leftIndex, 0, Width);
            rightIndex = MathHelper.Clamp(rightIndex, 0, Width);
            bottomIndex = MathHelper.Clamp(bottomIndex, 0, Height);
            topIndex = MathHelper.Clamp(topIndex, 0, Height);

            //if (CellBordersVisible)
            //    DrawCellBorders(leftIndex, bottomIndex, rightIndex, topIndex);

            GL.Enable(EnableCap.Texture2D);

            for (int layerNo = 0; layerNo < LayersNo; layerNo++)
            {
                primitiveRenderer.PushMatrix();
                primitiveRenderer.Translate(new Vector3(leftIndex * CellSize, bottomIndex * CellSize, 0.0f));

                for (int j = bottomIndex; j < topIndex; j++)
                {
                    primitiveRenderer.PushMatrix();

                    for (int i = leftIndex; i < rightIndex; i++)
                    {
                        var index = layerNo * Width * Height + i + Width * j;

                        var cellTile = Cells[index];

                        RenderCellTile(cellTile);

                        primitiveRenderer.Translate(new Vector3(CellSize, 0.0f, 0.0f));
                    }

                    primitiveRenderer.PopMatrix();
                    primitiveRenderer.Translate(new Vector3(0.0f, CellSize, 0.0f));
                }

                primitiveRenderer.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Public Methods

        #region Private Methods

        private TileCell[] CreateTileArray()
        {
            var cells = new TileCell[Width * Height * LayersNo];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = TileCell.Create();

            return cells;
        }

        ///// <summary>
        ///// Draw debug grid
        ///// </summary>
        ///// <param name="leftIndex">Left border index of grid</param>
        ///// <param name="bottomIndex">Bottom border index of grid</param>
        ///// <param name="rightIndex">Right border index of grid</param>
        ///// <param name="topIndex">Top border index of grid</param>
        //private void DrawCellBorders(int leftIndex, int bottomIndex, int rightIndex, int topIndex)
        //{
        //    GL.Color4(Color4.Green);

        //    for (int j = bottomIndex; j < topIndex; j++)
        //    {
        //        GL.Begin(PrimitiveType.Lines);
        //        GL.Vertex2(leftIndex * CellSize, j * CellSize);
        //        GL.Vertex2(rightIndex * CellSize, j * CellSize);
        //        GL.End();
        //    }

        //    for (int i = leftIndex; i < rightIndex; i++)
        //    {
        //        GL.Begin(PrimitiveType.Lines);
        //        GL.Vertex2(i * CellSize, bottomIndex * CellSize);
        //        GL.Vertex2(i * CellSize, topIndex * CellSize);
        //        GL.End();
        //    }
        //}

        private bool ValidateGridIndices(int xIndex, int yIndex)
        {
            if (xIndex < 0)
                return false;
            if (yIndex < 0)
                return false;
            if (xIndex >= Width)
                return false;
            if (yIndex >= Height)
                return false;

            return true;
        }

        private bool TryGetGridIndices(Vector2 point, out int xIndex, out int yIndex)
        {
            xIndex = (int)point.X / CellSize;
            yIndex = (int)point.Y / CellSize;
            if (xIndex < 0)
                return false;
            if (yIndex < 0)
                return false;
            if (xIndex >= Width)
                return false;
            if (yIndex >= Height)
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