using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Palettes.Readers;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class TileSetsDataHelper
    {
        public static TileSetModel FromBlkModel(DataProvider provider, ITileSetFromBlkEntry paletteData)
        {
            return provider.GetData(paletteData.DataRef) as TileSetModel;
        }

        public static TileSetModel FromImageModel(DataProvider provider, ITileSetFromImageEntry paletteData)
        {
            return null;
        }
    }
}
