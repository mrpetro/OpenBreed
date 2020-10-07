using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayoutModel
    {
        #region Internal Constructors

        internal MapLayoutModel(MapLayoutBuilder builder)
        {
            Width = builder.Width;
            Height = builder.Height;
            Layers = builder.Layers.Select(layer => layer.Build()).ToList();
        }

        #endregion Internal Constructors

        #region Public Properties

        public int Width { get; set; }
        public int Height { get; set; }

        public List<MapLayerModel> Layers { get; }

        #endregion Public Properties

        #region Public Methods

        public MapCellModel GetCell(int x, int y)
        {
            if (x > Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expecting 0 <= x < {Width}");
            if (y > Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expecting 0 <= y < {Height}");

            var valueIndex = y * Width + x;
            var values = new int[Layers.Count];

            for (int i = 0; i < Layers.Count; i++)
                values[i] = Layers[i].GetCellValue(valueIndex);

            return new MapCellModel(x, y, values);
        }

        #endregion Public Methods
    }
}