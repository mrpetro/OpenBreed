using OpenBreed.Common.Sources;
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

        public object Load(SourceBase source, List<FormatParameter> parameters)
        {
            var tileSize = (int)parameters.FirstOrDefault(item => item.Name =="TILE_SIZE").Value;
            var bitPlanesNo = (int)parameters.FirstOrDefault(item => item.Name == "BIT_PLANES_NO").Value; 

            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var reader = new ACBMTileSetReader(tileSetBuilder, tileSize, bitPlanesNo);
            return reader.Read(source.Stream);
        }

        public void Save(SourceBase source, object model)
        {
            throw new NotImplementedException("ACBMTileSet Write");
        }
    }
}
