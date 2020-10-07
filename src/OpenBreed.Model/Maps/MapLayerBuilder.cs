using System;

namespace OpenBreed.Model.Maps
{
    public class MapLayerBuilder
    {
        #region Private Fields

        private int width;
        private int height;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerBuilder(int width, int height, MapLayerType layerType)
        {
            this.width = width;
            this.height = height;
            LayerType = layerType;
            CellValues = new int[width * height];
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal MapLayerType LayerType { get; }
        internal int[] CellValues { get; }

        #endregion Internal Properties

        #region Public Methods

        public void SetValue(int x, int y, int value)
        {
            if (x < 0 || x >= width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expected in range from 0 to {width - 1}");

            if (y < 0 || y >= height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expected in range from 0 to {height - 1}");

            CellValues[y * width + x] = value;
        }

        public MapLayerModel Build()
        {
            return new MapLayerModel(this);
        }

        #endregion Public Methods
    }
}