using OpenBreed.Common.Images.Builders;
using OpenBreed.Common.Tiles.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Tiles.Readers.ACBM
{
    public class ACBMTileSetReader
    {
        #region Public Fields

        private int _bitPlanesNo;
        private int _tileSize;

        #endregion Public Fields

        #region Public Constructors

        public ACBMTileSetReader(TileSetBuilder builder, int tileSize, int bitPlanesNo)
        {
            Builder = builder;
            _tileSize = tileSize;
            _bitPlanesNo = bitPlanesNo;
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
            Builder.SetTileSize(_tileSize);
            Builder.SetPixelFormat(TilePixelFormat.Indexed8bpp);

            BinaryReader binReader = new BinaryReader(stream);
            binReader.BaseStream.Position = 0;
            int bytesPerTile = _tileSize * _tileSize / 8;

            int tilesNo = (int)binReader.BaseStream.Length / (bytesPerTile * _bitPlanesNo);

            if (tilesNo == 0)
                return null;


            for (int tileIndex = 0; tileIndex < tilesNo; tileIndex++)
            {
                byte[] tileData = new byte[_tileSize * _tileSize];

                for (int i = 0; i < _bitPlanesNo; i++)
                {
                    byte[] rawData = binReader.ReadBytes(bytesPerTile);
                    rawData = rawData.Reverse().ToArray();

                    if (rawData.Length == 0)
                        return null;

                    var bitArray = new BitArray(rawData);

                    for (int k = 0; k < _tileSize * _tileSize; k++)
                        tileData[k] += (byte)((bitArray[k] ? 1 : 0) << i);

                }

                tileData = tileData.Reverse().ToArray();

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
