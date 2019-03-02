using OpenBreed.Common.Assets;
using OpenBreed.Common.Tiles.Builders;
using OpenBreed.Common.Tiles.Readers.ACBM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class ACBMTileSetFormat : IDataFormatType
    {
        public ACBMTileSetFormat()
        {
        }

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            var tileSize = (int)parameters.FirstOrDefault(item => item.Name =="TILE_SIZE").Value;
            var bitPlanesNo = (int)parameters.FirstOrDefault(item => item.Name == "BIT_PLANES_NO").Value; 

            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var reader = new ACBMTileSetReader(tileSetBuilder, tileSize, bitPlanesNo);
            return reader.Read(asset.Stream);
        }

        public void Save(AssetBase source, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ACBMTileSet Write");
        }
    }
}
