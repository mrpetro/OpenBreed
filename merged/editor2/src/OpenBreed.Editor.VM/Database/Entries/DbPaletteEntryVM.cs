using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbPaletteEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbPalette _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbPaletteEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbPalette ?? throw new InvalidOperationException($"Expected {nameof(IDbPalette)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}