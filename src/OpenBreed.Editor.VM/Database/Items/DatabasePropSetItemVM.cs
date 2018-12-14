using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabasePropSetItemVM : DatabaseItemVM
    {
        private PropertySetDef _model;

        public DatabasePropSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            _model = itemDef as PropertySetDef ?? throw new InvalidOperationException($"Expected {nameof(PropertySetDef)}");

            base.Load(itemDef);
        }

        public override void Open()
        {
            Owner.Root.PropSetEditor.TryLoad(_model);
            Owner.OpenedItem = this;
        }
    }
}
