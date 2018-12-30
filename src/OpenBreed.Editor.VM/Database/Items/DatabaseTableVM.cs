using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables;

namespace OpenBreed.Editor.VM.Database.Items
{
    public abstract class DatabaseTableVM
    {

        #region Public Constructors

        public DatabaseTableVM(DatabaseVM owner)
        {
            Owner = owner;
        }

        #endregion Public Constructors

        #region Public Properties

        public abstract string Name { get; }
        public DatabaseVM Owner { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract IEnumerable<DatabaseItemVM> GetItems();

        public abstract void Load(IRepository repository);

        #endregion Public Methods

    }
}
