using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenBreed.Model.Tiles
{
    public class TileSetBuilder
    {
        internal int TileSize;
        internal List<TileModel> Tiles;
        internal TilePixelFormat PixelFormat;

        public TileSetBuilder()
        {
            Tiles = new List<TileModel>();
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

        public static TileSetBuilder NewTileSet()
        {
            return new TileSetBuilder();
        }

        public TileSetModel Build()
        {
            return new TileSetModel(this);
        }


    }
}
