using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DatabaseTableSelectorVM : BaseViewModel
    {

        #region Public Fields

        public readonly DatabaseViewerVM Parent;

        #endregion Public Fields

        #region Private Fields

        private int _currentIndex = -1;
        private DatabaseTableVM _currentTable = null;

        #endregion Private Fields

        #region Internal Constructors

        internal DatabaseTableSelectorVM(DatabaseViewerVM parent)
        {
            Parent = parent;

            Items = new BindingList<DatabaseTableVM>();
            Items.ListChanged += (s, a) => OnPropertyChanged(nameof(Items));

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public DatabaseTableVM CurrentItem
        {
            get { return _currentTable; }
            set { SetProperty(ref _currentTable, value); }
        }

        public BindingList<DatabaseTableVM> Items { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.Root.PropertyChanged += Root_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnDatabaseChanged(DatabaseVM database)
        {
            if (Parent.Root.Database != null)
                UpdateWithDatabaseItems(database);
            else
                UpdateWithNoItems();
        }

        private void Root_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var editor = sender as EditorVM;

            switch (e.PropertyName)
            {
                case nameof(editor.Database):
                    OnDatabaseChanged(editor.Database);
                    break;
                default:
                    break;
            }
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentIndex):
                    UpdateCurrentItem();
                    break;
                case nameof(CurrentItem):
                    UpdateCurrentIndex();
                    break;
                case nameof(Database.Items):
                    CurrentItem = Items.FirstOrDefault();
                    break;
                default:
                    break;
            }
        }

        private void UpdateCurrentIndex()
        {
            if (CurrentItem == null)
                CurrentIndex = -1;
            else
                CurrentIndex = Items.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Items[CurrentIndex];
        }


        private void UpdateWithDatabaseItems(DatabaseVM database)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();
                foreach (var item in database.GetTables())
                {
                    Items.Add(item);
                }
            });
        }
        private void UpdateWithNoItems()
        {
            Items.Clear();
        }

        #endregion Private Methods

    }
}
