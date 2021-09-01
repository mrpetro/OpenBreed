using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayerModel
    {
        #region Public Fields

        public const int UNSET_CELL = -1;

        #endregion Public Fields

        #region Private Fields

        private readonly int[] cellValues;

        #endregion Private Fields

        #region Internal Constructors

        internal MapLayerModel(MapLayerBuilder builder)
        {
            cellValues = builder.CellValues.ToArray();
            LayerType = builder.LayerType;
            Width = builder.Width;
            Height = builder.Height;
            IsVisible = true;
        }

        #endregion Internal Constructors

        #region Public Properties

        public MapLayerType LayerType { get; }
        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// NOTE: Editor specific property
        /// </summary>
        public bool IsVisible { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void GetClipIndices(RectangleF viewRect, out int xFrom, out int yFrom, out int xTo, out int yTo)
        {
            xFrom = 0;
            yFrom = 0;
            xTo = Width - 1;
            yTo = Height - 1;

            //xFrom = (int)(viewRect.Left / 16);
            //yFrom = (int)(viewRect.Bottom / 16);
            //xTo = (int)(viewRect.Right / 16);
            //yTo = (int)(viewRect.Top / 16);

            xFrom = Clamp(xFrom, 0, Width - 1);
            yFrom = Clamp(yFrom, 0, Height - 1);
            xTo = Clamp(xTo, 0, Width - 1);
            yTo = Clamp(yTo, 0, Height - 1);
        }

        /// <summary>
        /// Get Layer cell using single index
        /// </summary>
        /// <param name="cellIndex">Index of cell from list</param>
        /// <returns>Tile reference object</returns>
        public int GetCellValue(int cellIndex)
        {
            return cellValues[cellIndex];
        }

        public int GetValue(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expected in range from 0 to {Width - 1}");

            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expected in range from 0 to {Height - 1}");

            return cellValues[y * Width + x];
        }

        public void SetCellValue(int cellIndex, int value)
        {
            if (cellValues[cellIndex] == value)
                return;

            cellValues[cellIndex] = value;
        }

        public void SetValue(int x, int y, int value)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expected in range from 0 to {Width - 1}");

            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expected in range from 0 to {Height - 1}");

            cellValues[y * Width + x] = value;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// TODO: Move this out
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        #endregion Private Methods
    }
}