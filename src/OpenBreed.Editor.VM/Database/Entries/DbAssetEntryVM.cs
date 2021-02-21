using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbAssetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IAssetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbAssetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IAssetEntry ?? throw new InvalidOperationException($"Expected {nameof(IAssetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}