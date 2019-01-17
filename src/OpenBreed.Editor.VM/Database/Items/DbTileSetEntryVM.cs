using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DbTileSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private ITileSetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbTileSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ITileSetEntry ?? throw new InvalidOperationException($"Expected {nameof(ITileSetEntry)}");

            base.Load(entry);
        }


        #endregion Public Methods
    }
}
