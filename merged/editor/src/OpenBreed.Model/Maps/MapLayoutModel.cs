using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Model.Maps
{
    public class MapLayoutModel : IMapLayoutModel
    {
        private readonly IDrawingFactory drawingFactory;
        #region Internal Constructors

        internal MapLayoutModel(MapLayoutBuilder builder, IDrawingFactory drawingFactory)
        {
            this.drawingFactory = drawingFactory;
            CellSize = builder.CellSize;
            Width = builder.Width;
            Height = builder.Height;
            Layers = builder.Layers.Select(layer => layer.Build()).ToList();
            Bounds = new MyRectangleF(0.0f, 0.0f, CellSize * Width, CellSize * Height);
            IndexBounds = GetIndexRectangle(Bounds);
        }

        #endregion Internal Constructors

        #region Public Properties

        public static bool FlippedY { get; set; }
        public MyRectangleF Bounds { get; }
        public int CellSize { get; }
        public int Height { get; }
        public MyRectangle IndexBounds { get; }
        public List<MapLayerModel> Layers { get; }
        public int Width { get; }

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<(int X, int Y)> FindCellsWithValue(int layerIndex, int value)
        {
            for (int iy = 0; iy < Height; iy++)
            {
                for (int ix = 0; ix < Width; ix++)
                {
                    var cellValue = GetCellValue(layerIndex, ix, iy);

                    if (cellValue == value)
                        yield return (ix, iy);
                }
            }
        }

        public IEnumerable<(int X, int Y)> FindNeighbourCellsWithValue(int x, int y, int layerIndex, int value)
        {
            for (int iy = 0; iy < Height; iy++)
            {
                for (int ix = 0; ix < Width; ix++)
                {
                    var cellValue = GetCellValue(layerIndex, ix, iy);

                    if (cellValue == value)
                        yield return (ix, iy);
                }
            }
        }

        public int GetCellValue(int layerIndex, int x, int y)
        {
            if (x > Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expecting 0 <= x < {Width}");
            if (y > Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expecting 0 <= y < {Height}");

            var cellIndex = GetCellIndex(x, y);

            return Layers[layerIndex].GetCellValue(cellIndex);
        }

        public int[] GetCellValues(int x, int y)
        {
            if (x > Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expecting 0 <= x < {Width}");
            if (y > Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expecting 0 <= y < {Height}");

            var cellIndex = GetCellIndex(x, y);
            var values = new int[Layers.Count];

            for (int i = 0; i < Layers.Count; i++)
                values[i] = Layers[i].GetCellValue(cellIndex);

            return values;
        }

        public void GetClipIndices(MyRectangleF viewRect, out int xFrom, out int yFrom, out int xTo, out int yTo)
        {
            xFrom = 0;
            yFrom = 0;
            xTo = Width - 1;
            yTo = Height - 1;

            xFrom = Clamp(xFrom, 0, Width - 1);
            yFrom = Clamp(yFrom, 0, Height - 1);
            xTo = Clamp(xTo, 0, Width - 1);
            yTo = Clamp(yTo, 0, Height - 1);
        }

        public MyPoint GetIndexPoint(MyPoint point)
        {
            var x = point.X / CellSize;
            var y = point.Y / CellSize;

            if (point.X < 0)
                x--;

            if (point.Y < 0)
                y--;

            return new MyPoint(x, y);
        }

        public MyRectangle GetIndexRectangle(MyRectangleF rect)
        {
            return new MyRectangle((int)(rect.X / CellSize), (int)(rect.Y / CellSize), (int)(rect.Width / CellSize), (int)(rect.Height / CellSize));
        }

        public MapLayerModel GetLayer(MapLayerType layerType)
        {
            return Layers.First(layer => layer.LayerType == layerType);
        }

        public int GetLayerIndex(MapLayerType layerType)
        {
            var layer = Layers.First(item => item.LayerType == layerType);
            return Layers.IndexOf(layer);
        }

        public void SetCellValue(int layerIndex, int x, int y, int value)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Expected in range from 0 to {Width - 1}");

            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Expected in range from 0 to {Height - 1}");

            var cellIndex = GetCellIndex(x, y);

            Layers[layerIndex].SetCellValue(cellIndex, value);
        }

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

        #endregion Public Methods

        #region Private Methods

        internal int GetCellIndex(int x, int y)
        {
            if (FlippedY)
                return (Height - y - 1) * Width + x;
            else
                return y * Width + x;
        }

        #endregion Private Methods
    }
}