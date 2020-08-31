using OpenBreed.Common.Model.Tiles;
using OpenBreed.Common.Model.Tiles.Builders;
using OpenBreed.Common.Tiles;
using System.IO;

namespace OpenBreed.Common.Readers.BLK
{
    public class BLKReader
    {

        #region Public Fields

        public const int TILE_SIZE = 16;

        #endregion Public Fields

        #region Public Constructors

        public BLKReader(TileSetBuilder builder)
        {
            Builder = builder;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TilesReaded { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal TileSetBuilder Builder { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public TileSetModel Read(Stream stream)
        {
            Builder.SetTileSize(TILE_SIZE);
            Builder.SetPixelFormat(TilePixelFormat.Indexed8bpp);

            BinaryReader binReader = new BinaryReader(stream);
            binReader.BaseStream.Position = 0;

            int tilesNo = (int)(binReader.BaseStream.Length / 256);

            if (tilesNo == 0)
                return null;

            for (int tileIndex = 0; tileIndex < tilesNo; tileIndex++)
            {
                byte[] rawData = binReader.ReadBytes(TILE_SIZE * TILE_SIZE);

                if (rawData.Length == 0)
                    return null;

                byte[] tileData = new byte[TILE_SIZE * TILE_SIZE];

                for (int k = 0; k < TILE_SIZE * TILE_SIZE; k++)
                    tileData[k] = rawData[(k * 64) % (TILE_SIZE * TILE_SIZE) + k / 4];

                //Start building new tile
                var tileBuilder = TileBuilder.NewTile();
                tileBuilder.SetIndex(tileIndex);
                tileBuilder.SetData(tileData);

                //Add new tile to tileset being build
                Builder.AddTile(tileBuilder.Build());
            }

            return Builder.Build();
        }

        #endregion Public Methods

    }
}