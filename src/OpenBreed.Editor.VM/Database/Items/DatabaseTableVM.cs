using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables;

namespace OpenBreed.Editor.VM.Database.Items
{
    public abstract class DatabaseTableVM : DatabaseItemVM
    {
        public DatabaseTableVM(DatabaseVM owner) : base(owner)
        {
        }

        public abstract IEnumerable<DatabaseItemVM> GetItems();
    }
}
