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
    public class ACBMTileSetFormat : IDataFormat
    {
        public ACBMTileSetFormat()
        {
        }

        public object Load(BaseSource source, Dictionary<string, object> parameters = null)
        {
            var tileSize = (int)parameters["TILE_SIZE"];
            var bitPlanesNo = (int)parameters["BIT_PLANES_NO"];

            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var reader = new ACBMTileSetReader(tileSetBuilder, tileSize, bitPlanesNo);
            return reader.Read(source.Stream);
        }

        public void Save(BaseSource source, object model)
        {
            throw new NotImplementedException("ACBMTileSet Write");
        }
    }
}
