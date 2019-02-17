using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Actions;
using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbActionSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IActionSetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbActionSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IActionSetEntry ?? throw new InvalidOperationException($"Expected {nameof(IActionSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
