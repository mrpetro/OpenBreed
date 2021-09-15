using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class TileGrid
    {
        #region Public Constructors

        public TileGrid(int width, int height, int layersNo, int cellSize)
        {
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

        #region Private Methods

        private TileCell[] CreateTileArray()
        {
            var cells = new TileCell[Width * Height * LayersNo];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = TileCell.Create();

            return cells;
        }

        #endregion Private Methods
    }
}
