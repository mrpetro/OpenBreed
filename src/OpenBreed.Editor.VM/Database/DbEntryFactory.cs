using OpenBreed.Common;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.TileStamps;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEntryFactory
    {
        #region Public Methods

        public DbEntryVM Create(IDbEntry entry)
        {
            if (entry is IDbImage)
                return new DbImageEntryVM();
            else if (entry is IDbSound)
                return new DbSoundEntryVM();
            else if (entry is IDbMap)
                return new DbMapEntryVM();
            else if (entry is IDbDataSource)
                return new DbDataSourceEntryVM();
            else if (entry is IDbActionSet)
                return new DbActionSetEntryVM();
            else if (entry is IDbTileAtlas)
                return new DbTileSetEntryVM();
            else if (entry is IDbTileStamp)
                return new DbTileStampEntryVM();
            else if (entry is IDbSpriteAtlas)
                return new DbSpriteSetEntryVM();
            else if (entry is IDbPalette)
                return new DbPaletteEntryVM();
            else if (entry is IDbText)
                return new DbTextEntryVM();
            else if (entry is IDbScript)
                return new DbScriptEntryVM();
            else if (entry is IDbEntityTemplate)
                return new DbEntityTemplateEntryVM();
            else if (entry is IDbAnimation)
                return new DbAnimationEntryVM();
            else
                throw new NotImplementedException(entry.ToString());
        }

        #endregion Public Methods

    }
}
