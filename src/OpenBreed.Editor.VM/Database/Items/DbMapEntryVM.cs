using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Maps;
using OpenBreed.Common.Maps;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DbMapEntryVM : DbEntryVM
    {
        #region Private Fields

        private IMapEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbMapEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IMapEntry ?? throw new InvalidOperationException($"Expected {nameof(IMapEntry)}");

            base.Load(entry);     
        }

        #endregion Public Methods
    }
}
