using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
using OpenBreed.Common.Sprites;
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
        #region Public Properties

        public MapModel Map { get; internal set; }
        public List<PaletteModel> Palettes { get; } = new List<PaletteModel>();
        public IPropSetEntry PropSet { get; internal set; }
        public List<TileSetModel> TileSets { get; } = new List<TileSetModel>();
        public List<SpriteSetModel> SpriteSets { get; } = new List<SpriteSetModel>();
        #endregion Public Properties
    }
}
