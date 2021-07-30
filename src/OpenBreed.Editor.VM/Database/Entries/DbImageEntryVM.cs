using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbImageEntryVM : DbEntryVM
    {
        #region Private Fields

        private string _assetId;


        private IDbImage _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbImageEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        public string AssetId
        {
            get { return _assetId; }
            set { SetProperty(ref _assetId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbImage ?? throw new InvalidOperationException($"Expected {nameof(IDbImage)}");
            AssetId = _entry.DataRef;

            base.Load(entry);
        }

        #endregion Public Methods
    }
}