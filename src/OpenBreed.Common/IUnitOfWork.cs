using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Palettes;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Items.Sprites;
using OpenBreed.Common.Database.Items.Tiles;
using OpenBreed.Common.Database.Tables;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : IEntity;
        void Save();
        LevelDef GetLevelDef(string levelName);
        PaletteDef GetPaletteDef(string paletteName);
        IEnumerable<DatabaseTableDef> GetTables();
    }
}
