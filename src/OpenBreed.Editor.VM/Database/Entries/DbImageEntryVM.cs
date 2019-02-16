using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.Images;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbImageEntryVM : DbEntryVM
    {
        #region Private Fields

        private string _assetId;

        #endregion Private Fields

        #region Public Constructors

        public DbImageEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            var imageEntry = entry as IImageEntry ?? throw new InvalidOperationException($"Expected {nameof(IImageEntry)}");
            AssetId = imageEntry.AssetRef;

            base.Load(entry);
        }

        #endregion Public Methods

        public string AssetId
        {
            get { return _assetId; }
            set { SetProperty(ref _assetId, value); }
        }

    }
}
