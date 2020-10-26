using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenBreed.Model.Tiles
{
    public class TileSetBuilder
    {
        #region Internal Fields

        internal int TileSize;
        internal List<TileModel> Tiles;
        internal TilePixelFormat PixelFormat;
        internal Bitmap Bitmap;
        internal int TilesNoX;
        internal int TilesNoY;

        #endregion Internal Fields

        #region Public Constructors

        public TileSetBuilder()
        {
            Tiles = new List<TileModel>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static TileSetBuilder NewTileSet()
        {
            return new TileSetBuilder();
        }

        public void SetTileSize(int tileSize)
        {
            TileSize = tileSize;
        }

        public void SetPixelFormat(TilePixelFormat pixelFormat)
        {
            PixelFormat = pixelFormat;
        }

        public TileSetBuilder AddTile(TileModel tile)
        {
            Tiles.Add(tile);
            return this;
        }

        public TileSetModel Build()
        {
            Bitmap = ToBitmap(Tiles);
            RebuildRectangles();

            return new TileSetModel(this);
        }

        #endregion Public Methods

        #region Private Methods

        private void RebuildRectangles()
        {
            var tilesCount = TilesNoX * TilesNoY;

            for (int tileId = 0; tileId < tilesCount; tileId++)
            {
                int tileIndexX = tileId % TilesNoX;
                int tileIndexY = tileId / TilesNoX;
                Tiles[tileId].Rectangle = new Rectangle(tileIndexX * TileSize, tileIndexY * TileSize, TileSize, TileSize);
            }
        }

        private void CreateDefaultBitmap()
        {
            Bitmap = new Bitmap(320, 768, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            TileSize = 16;
            TilesNoX = Bitmap.Width / TileSize;
            TilesNoY = Bitmap.Height / TileSize;

            using (Graphics gfx = Graphics.FromImage(Bitmap))
            {
                Font font = new Font("Arial", 5);

                for (int j = 0; j < TilesNoY; j++)
                {
                    for (int i = 0; i < TilesNoX; i++)
                    {
                        int tileId = i + j * TilesNoX;

                        var rectangle = new Rectangle(i * TileSize, j * TileSize, TileSize - 1, TileSize - 1);

                        Color c = Color.Black;
                        Pen tileColor = new Pen(c);
                        Brush brush = new SolidBrush(c);

                        gfx.FillRectangle(brush, rectangle);

                        c = Color.White;
                        tileColor = new Pen(c);
                        brush = new SolidBrush(c);

                        gfx.DrawRectangle(tileColor, rectangle);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId / 100), font, brush, i * TileSize + 2, j * TileSize + 1);
                        gfx.DrawString(string.Format("{0,2:D2}", tileId % 100), font, brush, i * TileSize + 2, j * TileSize + 7);
                    }
                }
            }
        }

        private Bitmap ToBitmap(List<TileModel> tiles)
        {
            int bmpWidth = 320;
            TilesNoX = bmpWidth / TileSize;
            TilesNoY = tiles.Count / TilesNoX;
            int bmpHeight = TilesNoY * TileSize;
            var bitmap = new Bitmap(bmpWidth, bmpHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            for (int j = 0; j < TilesNoY; j++)
            {
                for (int i = 0; i < TilesNoX; i++)
                {
                    //Create a BitmapData and Lock all pixels to be written
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(i * TileSize, j * TileSize, TileSize, TileSize),
                                                         ImageLockMode.WriteOnly, bitmap.PixelFormat);

                    //Copy the data from the byte array into BitmapData.Scan0
                    for (int k = 0; k < TileSize; k++)
                        Marshal.Copy(tiles[i + j * TilesNoX].Data, k * TileSize, bmpData.Scan0 + k * bmpData.Stride, TileSize);

                    //Unlock the pixels
                    bitmap.UnlockBits(bmpData);
                }
            }

            return bitmap;
        }

        #endregion Private Methods
    }
}