using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Editor.VM.Database.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DatabaseTableViewerVM : BaseViewModel
    {

        #region Public Fields

        public readonly DatabaseViewerVM Parent;

        #endregion Public Fields

        #region Internal Constructors

        internal DatabaseTableViewerVM(DatabaseViewerVM parent)
        {
            Parent = parent;

            Items = new BindingList<DatabaseEntryVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));
        }

        #endregion Internal Constructors

        #region Public Properties

        public BindingList<DatabaseEntryVM> Items { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.TableSelector.PropertyChanged += TableSelector_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCurrentTableChanged(DatabaseTableVM currentTable)
        {
            if (currentTable != null)
                UpdateWithCurrentTableItems(currentTable);
            else
                UpdateWithNoItems();
        }

        private void TableSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tableSelector = sender as DatabaseTableSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(tableSelector.CurrentItem):
                    OnCurrentTableChanged(tableSelector.CurrentItem);
                    break;
                default:
                    break;
            }
        }
        private void UpdateWithCurrentTableItems(DatabaseTableVM currentTable)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
                foreach (var item in currentTable.GetItems())
                    Items.Add(item);
            });
        }

        private void UpdateWithNoItems()
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
            });
        }

        #endregion Private Methods
    }
}