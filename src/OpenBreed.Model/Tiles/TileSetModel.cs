using OpenBreed.Common.Interface.Drawing;
using System.Collections.Generic;

namespace OpenBreed.Model.Tiles
{
    public enum TilePixelFormat
    {
        Indexed8bpp
    }

    public class TileSetModel
    {
        #region Internal Constructors

        internal TileSetModel(TileSetBuilder builder)
        {
            Tiles = builder.Tiles;
            TileSize = builder.TileSize;
            TilesNoX = builder.TilesNoX;
            TilesNoY = builder.TilesNoY;
            PixelFormat = builder.PixelFormat;
            Bitmap = builder.Bitmap;
        }

        #endregion Internal Constructors

        #region Public Properties

        public byte[] Bitmap { get; }

        public TilePixelFormat PixelFormat { get; private set; }

        /// <summary>
        ///  Gets or sets an object that provides additional data context.
        /// </summary>
        public object Tag { get; set; }

        public List<TileModel> Tiles { get; }

        public int TileSize { get; }

        public int TilesNoX { get; }

        public int TilesNoY { get; }

        #endregion Public Properties

        #region Public Methods

        public MyPoint GetIndexCoords(MyPoint point)
        {
            return new MyPoint(point.X / TileSize, point.Y / TileSize);
        }

        public MyPoint GetSnapCoords(MyPoint point)
        {
            int x = point.X / TileSize;
            int y = point.Y / TileSize;

            return new MyPoint(x * TileSize, y * TileSize);
        }

        public List<int> GetTileIdList(MyRectangle rectangle)
        {
            int left = rectangle.Left;
            int right = rectangle.Right;
            int top = rectangle.Top;
            int bottom = rectangle.Bottom;

            if (left < 0)
                left = 0;

            if (right > TileSize * TilesNoX)
                right = TileSize * TilesNoX;

            if (top < 0)
                top = 0;

            if (bottom > TileSize * TilesNoY)
                bottom = TileSize * TilesNoY;

            rectangle = new MyRectangle(left, top, right - left, bottom - top);

            var tileIdList = new List<int>();
            int xFrom = rectangle.Left / TileSize;
            int xTo = rectangle.Right / TileSize;
            int yFrom = rectangle.Top / TileSize;
            int yTo = rectangle.Bottom / TileSize;

            for (int xIndex = xFrom; xIndex < xTo; xIndex++)
            {
                for (int yIndex = yFrom; yIndex < yTo; yIndex++)
                {
                    int gfxId = xIndex + TilesNoX * yIndex;
                    tileIdList.Add(gfxId);
                }
            }

            return tileIdList;
        }

        #endregion Public Methods
    }
}