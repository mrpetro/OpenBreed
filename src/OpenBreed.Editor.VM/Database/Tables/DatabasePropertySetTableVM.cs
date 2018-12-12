using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabasePropertySetTableVM : DatabaseTableVM
    {
        #region Private Fields

        private DatabasePropertySetTableDef _model;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePropertySetTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Property sets"; } }

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
            _model = itemDef as DatabasePropertySetTableDef ?? throw new InvalidOperationException($"Expected {nameof(DatabasePropertySetTableDef)}");

            base.Load(itemDef);
        }

        #endregion Public Methods
    }
}
