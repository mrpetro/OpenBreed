using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEntryFactory
    {
        #region Public Methods

        public DbEntryVM Create(IEntry entry)
        {
            if (entry is IImageEntry)
                return new DbImageEntryVM();
            else if (entry is ISoundEntry)
                return new DbSoundEntryVM();
            else if (entry is IMapEntry)
                return new DbMapEntryVM();
            else if (entry is IAssetEntry)
                return new DbAssetEntryVM();
            else if (entry is IActionSetEntry)
                return new DbActionSetEntryVM();
            else if (entry is ITileSetEntry)
                return new DbTileSetEntryVM();
            else if (entry is ISpriteSetEntry)
                return new DbSpriteSetEntryVM();
            else if (entry is IPaletteEntry)
                return new DbPaletteEntryVM();
            else
                throw new NotImplementedException(entry.ToString());
        }

        #endregion Public Methods

    }
}
