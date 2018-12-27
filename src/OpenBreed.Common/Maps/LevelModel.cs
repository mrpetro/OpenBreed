using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class LevelModel
    {
        public List<TileSetModel> TileSets { get; }
        public PropSetModel PropSet { get; }
        public List<PaletteModel> Palettes { get; }


    }
}
