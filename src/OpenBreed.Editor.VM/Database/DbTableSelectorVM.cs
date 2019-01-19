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
    public class DbTableSelectorVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private string _currentTable = null;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableSelectorVM()
        {
            Items = new BindingList<string>();
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

        public string CurrentItem
        {
            get { return _currentTable; }
            set { SetProperty(ref _currentTable, value); }
        }

        public BindingList<string> Items { get; }

        #endregion Public Properties

        #region Private Methods

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

        #endregion Private Methods

    }
}
