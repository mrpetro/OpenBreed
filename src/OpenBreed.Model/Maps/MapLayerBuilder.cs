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
            for (int i = 0; i < CellValues.Length; i++)
                CellValues[i] = MapLayerModel.UNSET_CELL;
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

        public int GetValue(int x, int y)
        {
            return CellValues[y * Width + x];
        }

        public MapLayerModel Build()
        {
            return new MapLayerModel(this);
        }

        internal void GenerateGroups(MapLayerBuilder layer)
        {
            int nextGroup = 0;

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    var value = layer.GetValue(x, y);

                    //Ignore "Air" and "Obstracle" cells
                    if (value == 0 || value == 63)
                        continue;

                    var currentGroup = GetValue(x, y);

                    //Check if group already set (processed)
                    if (currentGroup == MapLayerModel.UNSET_CELL)
                    {
                        Flood(layer, x, y, value, nextGroup);
                        nextGroup++;
                    }
                }
            }
        }

        private void Flood(MapLayerBuilder layer, int x, int y, int valueToFind, int groupId)
        {
            //Check if group already set (processed)
            if (GetValue(x, y) == MapLayerModel.UNSET_CELL && layer.GetValue(x, y) == valueToFind)
            {
                SetValue(x, y, groupId);
                if  (x + 1 < Width) Flood(layer, x + 1, y, valueToFind, groupId);
                if (y + 1 < Height) Flood(layer, x, y + 1, valueToFind, groupId);
                if (x - 1 >= 0) Flood(layer, x - 1, y, valueToFind, groupId);
                if (y - 1 >= 0) Flood(layer, x, y - 1, valueToFind, groupId);
            }
        }

        #endregion Public Methods
    }
}