using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbAssetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbAsset _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbAssetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbAsset ?? throw new InvalidOperationException($"Expected {nameof(IDbAsset)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}