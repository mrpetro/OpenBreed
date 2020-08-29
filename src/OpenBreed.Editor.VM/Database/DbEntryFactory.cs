﻿using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Texts;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;

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
            else if (entry is IDataSourceEntry)
                return new DbDataSourceEntryVM();
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
            else if (entry is ITextEntry)
                return new DbTextEntryVM();
            else
                throw new NotImplementedException(entry.ToString());
        }

        #endregion Public Methods

    }
}
