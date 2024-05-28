using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEntriesEditorVM : BaseViewModel
    {
        #region Private Fields

        private EntryEditorsViewItemVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public DbEntriesEditorVM()
        {
            Items = new ObservableCollection<EntryEditorsViewItemVM>();
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryEditorsViewItemVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public ObservableCollection<EntryEditorsViewItemVM> Items { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddEditor(string editorKey, EntryEditorVM entryEditor)
        {
            var itemVM = new EntryEditorsViewItemVM(entryEditor, OnCloseEditor);

            Items.Add(itemVM);

            CurrentItem = itemVM;
        }

        public void ActivateEditor(EntryEditorVM entryEditor)
        {
            var editorItem = Items.FirstOrDefault(item => item.Editor == entryEditor);

            if (editorItem is null)
            {
                throw new InvalidOperationException("Expected editor item to be found!");
            }

            entryEditor.Activate();
            CurrentItem = editorItem;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCloseEditor(EntryEditorsViewItemVM itemVM)
        {
            var index = Items.IndexOf(itemVM);

            Items.Remove(itemVM);

            if (Items.Any())
            {
                if (index >= Items.Count)
                {
                    index--;
                }

                CurrentItem = Items[index];
            }
            else
            {
                CurrentItem = null;
            }
        }

        #endregion Private Methods
    }
}