using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class TileGridBuilder : ITileGridBuilder
    {
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        internal int CellSize { get; private set; }
        internal int LayersNo { get; private set; }
        internal bool CellBordersVisible { get; private set; }

        public ITileGridBuilder SetSize(int width, int height)
        {
            if (width <= 0)
                throw new InvalidOperationException("Width must be set to greater than zero.");

            if (height <= 0)
                throw new InvalidOperationException("Height must be set to greater than zero.");

            Width = width;
            Height = height;

            return this;
        }

        public ITileGridBuilder SetLayersNo(int layersNo)
        {
            if (layersNo <= 0)
                throw new InvalidOperationException("layersNo must be set to greater than zero.");

            LayersNo = layersNo;

            return this;
        }

        public ITileGridBuilder SetCellSize(int cellSize)
        {
            if (cellSize <= 0)
                throw new InvalidOperationException("CellSize must be set to greater than zero.");

            CellSize = cellSize;

            return this;
        }

        public TileCell[] CreateTileArray()
        {
            var cells = new TileCell[Width * Height * LayersNo];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = TileCell.Create();

            return cells;
        }

        public ITileGrid Build()
        {
            return new TileGrid(this);
        }
    }
}
