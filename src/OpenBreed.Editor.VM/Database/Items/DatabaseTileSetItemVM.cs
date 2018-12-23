using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseTileSetItemVM : DatabaseItemVM
    {
        private TileSetDef _model;

        public DatabaseTileSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            _model = itemDef as TileSetDef ?? throw new InvalidOperationException($"Expected {nameof(TileSetDef)}");

            base.Load(itemDef);
        }

        public override void Open()
        {
            Owner.Root.TileSetEditor.TryLoad(_model);
            Owner.OpenedItem = this;
        }
    }
}
