using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class DbTableFactory
    {
        public DbTableVM CreateTable(IRepository repository)
        {
            if (repository is IRepository<IImageEntry>)
                return new DbImageTableVM();
            if (repository is IRepository<ISoundEntry>)
                return new DbSoundTableVM();
            else if (repository is IRepository<IMapEntry>)
                return new DbMapTableVM();
            else if (repository is IRepository<IActionSetEntry>)
                return new DbActionSetTableVM();
            else if (repository is IRepository<IAssetEntry>)
                return new DbAssetTableVM();
            else if (repository is IRepository<ITileSetEntry>)
                return new DbTileSetTableVM();
            else if (repository is IRepository<ISpriteSetEntry>)
                return new DbSpriteSetTableVM();
            else if (repository is IRepository<IPaletteEntry>)
                return new DbPaletteTableVM();
            else
                throw new NotImplementedException(repository.ToString());
        }
    }
}
