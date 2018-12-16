using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseTileSetTableVM : DatabaseTableVM
    {
        #region Private Fields

        private DatabaseTileSetTableDef _model;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseTileSetTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Tile sets"; } }

        #endregion Public Properties

        #region Public Methods

        public override IEnumerable<DatabaseItemVM> GetItems()
        {
            foreach (var itemDef in _model.Items)
            {
                var itemVM = Owner.CreateItem(itemDef);
                itemVM.Load(itemDef);
                yield return itemVM;
            }
        }

        public override void Load(DatabaseItemDef itemDef)
        {
            _model = itemDef as DatabaseTileSetTableDef ?? throw new InvalidOperationException($"Expected {nameof(DatabaseTileSetTableDef)}");

            base.Load(itemDef);
        }

        #endregion Public Methods
    }
}
