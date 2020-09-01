using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
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

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ITileSetEntry ?? throw new InvalidOperationException($"Expected {nameof(ITileSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}