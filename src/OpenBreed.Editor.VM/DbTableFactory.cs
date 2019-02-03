using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
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
                return new DatabaseImageTableVM();
            if (repository is IRepository<ISoundEntry>)
                return new DatabaseSoundTableVM();
            else if (repository is IRepository<IMapEntry>)
                return new DatabaseMapTableVM();
            else if (repository is IRepository<IPropSetEntry>)
                return new DatabasePropertySetTableVM();
            else if (repository is IRepository<IAssetEntry>)
                return new DatabaseAssetTableVM();
            else if (repository is IRepository<ITileSetEntry>)
                return new DatabaseTileSetTableVM();
            else if (repository is IRepository<ISpriteSetEntry>)
                return new DatabaseSpriteSetTableVM();
            else if (repository is IRepository<IPaletteEntry>)
                return new DatabasePaletteTableVM();
            else
                throw new NotImplementedException(repository.ToString());
        }
    }
}
