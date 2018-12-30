using OpenBreed.Common;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
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

        private ISourceEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSourceItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as ISourceEntity ?? throw new InvalidOperationException($"Expected {nameof(ISourceEntity)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
