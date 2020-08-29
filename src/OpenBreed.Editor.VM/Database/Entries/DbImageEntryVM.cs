using OpenBreed.Common;
using OpenBreed.Common.Images;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbImageEntryVM : DbEntryVM
    {
        #region Private Fields

        private string _assetId;


        private IImageEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbImageEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        public string AssetId
        {
            get { return _assetId; }
            set { SetProperty(ref _assetId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IImageEntry ?? throw new InvalidOperationException($"Expected {nameof(IImageEntry)}");
            AssetId = _entry.DataRef;

            base.Load(entry);
        }

        #endregion Public Methods
    }
}