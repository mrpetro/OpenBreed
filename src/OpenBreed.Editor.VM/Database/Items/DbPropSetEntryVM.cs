using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DbPropSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IPropSetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbPropSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IPropSetEntry ?? throw new InvalidOperationException($"Expected {nameof(IPropSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}
