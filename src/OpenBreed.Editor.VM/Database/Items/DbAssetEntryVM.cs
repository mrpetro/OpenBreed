using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
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

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IAssetEntry ?? throw new InvalidOperationException($"Expected {nameof(IAssetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
