using System;

namespace OpenBreed.Model.Maps
{
    public class MapLayerBuilder
    {
        #region Private Fields

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerBuilder(int width, int height, MapLayerType layerType)
        {
            this.Width = width;
            this.Height = height;
            LayerType = layerType;
            CellValues = new int[width * height];
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal MapLayerType LayerType { get; }
        internal int[] CellValues { get; }
        internal int Width { get; }
        internal int Height { get; }

        #endregion Internal Properties

        #region Public Methods

        public void SetValue(int x, int y, int value)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expected in range from 0 to {Width - 1}");

            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expected in range from 0 to {Height - 1}");

            CellValues[y * Width + x] = value;
        }

        public MapLayerModel Build()
        {
            return new MapLayerModel(this);
        }

        #endregion Public Methods
    }
}