using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayoutModel : IMapLayoutModel
    {
        #region Internal Constructors

        internal MapLayoutModel(MapLayoutBuilder builder)
        {
            CellSize = builder.CellSize;
            Width = builder.Width;
            Height = builder.Height;
            Layers = builder.Layers.Select(layer => layer.Build()).ToList();
            Bounds = new RectangleF(0, 0, CellSize * Width, CellSize * Height);
        }

        #endregion Internal Constructors

        #region Public Properties

        public RectangleF Bounds { get; }

        public int CellSize { get; }
        public int Width { get; }
        public int Height { get; }

        public List<MapLayerModel> Layers { get; }

        #endregion Public Properties

        #region Public Methods

        public int GetLayerIndex(MapLayerType layerType)
        {
            var layer = Layers.First(item => item.LayerType == layerType);
            return Layers.IndexOf(layer);
        }

        public int[] GetCellValues(int x, int y)
        {
            if (x > Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expecting 0 <= x < {Width}");
            if (y > Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expecting 0 <= y < {Height}");

            var valueIndex = y * Width + x;
            var values = new int[Layers.Count];

            for (int i = 0; i < Layers.Count; i++)
                values[i] = Layers[i].GetCellValue(valueIndex);

            return values;
        }

        #endregion Public Methods
    }
}