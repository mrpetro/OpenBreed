using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableSelectorConnector : VMConnectorBase<DbTableSelectorVM>
    {
        #region Public Constructors

        public DbTableSelectorConnector(DbTableSelectorVM source) : base(source)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void ConnectTo(DbEditorVM dbEditor)
        {
            dbEditor.PropertyChanged += DbEditor_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void DbEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var dbEditor = sender as DbEditorVM;

            switch (e.PropertyName)
            {
                case nameof(dbEditor.Editable):
                    OnDatabaseChanged(dbEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnDatabaseChanged(DatabaseVM db)
        {
            if (db != null)
                UpdateWithDbTables(db);
            else
                UpdateWithNoItems();
        }

        private void UpdateWithDbTables(DatabaseVM db)
        {
            Source.Items.UpdateAfter(() =>
            {
                Source.Items.Clear();
                foreach (var item in db.GetTables())
                    Source.Items.Add(item);
            });
        }
        private void UpdateWithNoItems()
        {
            Source.Items.Clear();
        }

        #endregion Private Methods
    }
}
