using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseSourceItemVM : DatabaseItemVM
    {
        public DatabaseSourceItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            var sourceDef = itemDef as SourceDef ?? throw new InvalidOperationException($"Expected {nameof(SourceDef)}");

            base.Load(itemDef);
        }
    }
}
