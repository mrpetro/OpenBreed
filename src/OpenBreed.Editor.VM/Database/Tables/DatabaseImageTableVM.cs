using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseImageTableVM : DatabaseTableVM
    {

        #region Private Fields

        private DatabaseImageTableDef _model;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseImageTableVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Images"; } }

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
            _model = itemDef as DatabaseImageTableDef ?? throw new InvalidOperationException($"Expected {nameof(DatabaseImageTableDef)}");

            base.Load(itemDef);
        }

        #endregion Public Methods
    }
}
