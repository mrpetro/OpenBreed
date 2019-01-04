using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseSourceItemVM : DatabaseItemVM
    {
        #region Private Fields

        private ISourceEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSourceItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as ISourceEntry ?? throw new InvalidOperationException($"Expected {nameof(ISourceEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
