using OpenBreed.Database.Interface;
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

        private readonly IRepositoryProvider repositoryProvider;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private EntryEditorsViewItemVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public DbEntriesEditorVM(
            IRepositoryProvider repositoryProvider,
            DbEntryEditorFactory dbEntryEditorFactory)
        {
            this.repositoryProvider = repositoryProvider;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<EntryEditorVM> EntryEditorOpeningAction { get; set; }

        public EntryEditorsViewItemVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public ObservableCollection<EntryEditorsViewItemVM> Items { get; } = new ObservableCollection<EntryEditorsViewItemVM>();

        #endregion Public Properties

        #region Public Methods

        public void OpenOrActivateEditor()
        {
        }

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

        #region Internal Methods

        internal EntryEditorVM OpenOrActivateEditor(string tableName, string entryId)
        {
            string entryEditorKey = $"{tableName}#{entryId}";

            var entryEditor = Items.Select(item => item.Editor).FirstOrDefault(item => item.IsEdited(tableName, entryId));

            if (entryEditor is null)
            {
                var repository = repositoryProvider.GetRepository(tableName);

                var entry = repository.Find(entryId);

                entryEditor = dbEntryEditorFactory.Create(entry);

                AddEditor(entryEditorKey, entryEditor);

                entryEditor.EditEntry(entryId);
                EntryEditorOpeningAction?.Invoke(entryEditor);
            }
            else
            {
                ActivateEditor(entryEditor);
            }

            return entryEditor;
        }

        public void CloseAll()
        {
            var toClose = Items.Select(item => item.Editor).ToArray();

            foreach (var entryEditor in toClose)
                entryEditor.Close();
        }

        #endregion Internal Methods

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