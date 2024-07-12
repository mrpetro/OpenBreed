using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbTileSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbTileAtlas _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbTileSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbTileAtlas ?? throw new InvalidOperationException($"Expected {nameof(IDbTileAtlas)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}