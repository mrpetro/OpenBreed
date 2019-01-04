using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.Images;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseImageItemVM : DatabaseItemVM
    {
        #region Private Fields

        private IImageEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseImageItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IImageEntry ?? throw new InvalidOperationException($"Expected {nameof(IImageEntry)}");

            base.Load(entry);
        }

        public override void Open()
        {
            Owner.Root.ImageViewer.OpenEntry(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}
