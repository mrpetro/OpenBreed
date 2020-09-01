using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbPaletteEntryVM : DbEntryVM
    {
        #region Private Fields

        private IPaletteEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbPaletteEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IPaletteEntry ?? throw new InvalidOperationException($"Expected {nameof(IPaletteEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}