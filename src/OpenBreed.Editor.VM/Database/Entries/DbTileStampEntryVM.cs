using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.TileStamps;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbTileStampEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbTileStamp _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbTileStampEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbTileStamp ?? throw new InvalidOperationException($"Expected {nameof(IDbTileStamp)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}