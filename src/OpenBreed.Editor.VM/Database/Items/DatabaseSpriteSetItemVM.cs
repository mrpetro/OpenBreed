using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseSpriteSetItemVM : DatabaseItemVM
    {
        private SpriteSetDef _model;

        public DatabaseSpriteSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            _model = itemDef as SpriteSetDef ?? throw new InvalidOperationException($"Expected {nameof(SpriteSetDef)}");

            base.Load(itemDef);
        }

        public override void Open()
        {
            throw new NotImplementedException();

            //Owner.Root.PropSetEditor.TryLoad(_model);
            Owner.OpenedItem = this;
        }
    }
}
