using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Readers.BLK;
using OpenBreed.Common.Tiles.Builders;
using OpenBreed.Common.Assets;

namespace OpenBreed.Common.Formats
{
    public class ABTABLKFormat : IDataFormatType
    {
        public ABTABLKFormat()
        {
        }

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            BLKReader blkReader = new BLKReader(tileSetBuilder);
            return blkReader.Read(asset.Stream);
        }

        public void Save(AssetBase source, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ABTABLK Write");
        }
    }
}
