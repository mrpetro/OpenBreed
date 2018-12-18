using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Palettes;
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
    public class DatabasePaletteItemVM : DatabaseItemVM
    {
        private PaletteDef _model;

        public DatabasePaletteItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            _model = itemDef as PaletteDef ?? throw new InvalidOperationException($"Expected {nameof(PaletteDef)}");

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
