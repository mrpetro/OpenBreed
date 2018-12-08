using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseSourceTableVM : DatabaseTableVM
    {

        #region Private Fields

        private DatabaseSourceTableDef _model;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSourceTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Sources"; } }

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
            _model = itemDef as DatabaseSourceTableDef ?? throw new InvalidOperationException($"Expected {nameof(DatabaseSourceTableDef)}");

            base.Load(itemDef);
        }

        #endregion Public Methods

    }
}
