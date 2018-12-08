using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Images;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseImageItemVM : DatabaseItemVM
    {
        public DatabaseImageItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            var imageDef = itemDef as ImageDef ?? throw new InvalidOperationException($"Expected {nameof(ImageDef)}");

            base.Load(itemDef);
        }
    }
}
