using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly Dictionary<string, EntryEditorVM> _openedEntryEditors = new Dictionary<string, EntryEditorVM>();
        private readonly IServiceProvider managerCollection;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IRepositoryProvider repositoryProvider;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM(IServiceProvider managerCollection,
                          DbEntryEditorFactory dbEntryEditorFactory,
                          IRepositoryProvider repositoryProvider)
        {
            this.managerCollection = managerCollection;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<EntryEditorVM> EntryEditorOpeningAction { get; set; }

        public bool IsModified { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public void CloseAllEditors()
        {
            var toClose = _openedEntryEditors.Values.ToArray();

            foreach (var entryEditor in toClose)
                entryEditor.Close();

            CloseDbTablesEditor();
        }

        private DbTablesEditorVM dbTablesEditor;

        private bool dbTablesEditorChecked;

        public Action<DbTablesEditorVM> InitDbTablesEditorAction { get; set; }

        public bool DbTablesEditorChecked
        {
            get { return dbTablesEditorChecked; }
            set { SetProperty(ref dbTablesEditorChecked, value); }
        }

        public void CloseDbTablesEditor()
        {
            dbTablesEditor.Close();
            dbTablesEditor = null;
            DbTablesEditorChecked = false;
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DbTablesEditorChecked):
                    ToggleDbTablesEditor(DbTablesEditorChecked);
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        public void ToggleDbTablesEditor(bool toggle)
        {
            if (dbTablesEditor == null)
            {
                dbTablesEditor = managerCollection.GetService<DbTablesEditorVM>();
                dbTablesEditor.EntryEditorOpener = OpenEntryEditor;
                InitDbTablesEditorAction?.Invoke(dbTablesEditor);

                dbTablesEditor.DbTableEditor.SetModel(dbTablesEditor.DbTableSelector.CurrentTableName);

                dbTablesEditor.PropertyChanged += (s, a) => { if (Equals(a.PropertyName, "IsHidden")) DbTablesEditorChecked = !dbTablesEditor.IsHidden; };
            }

            dbTablesEditor.IsHidden = !toggle;
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM OpenEntryEditor(string tableName, string entryId)
        {
            var repository = repositoryProvider.GetRepository(tableName);

            string entryEditorKey = $"{tableName}#{entryId}";

            EntryEditorVM entryEditor = null;
            if (!_openedEntryEditors.TryGetValue(entryEditorKey, out entryEditor))
            {
                var entry = repository.Find(entryId);

                entryEditor = dbEntryEditorFactory.Create(repository, entry);
                _openedEntryEditors.Add(entryEditorKey, entryEditor);
                entryEditor.ClosedAction = () => OnEntryEditorClosed(entryEditor);
                entryEditor.EditEntry(entryId);
                EntryEditorOpeningAction?.Invoke(entryEditor);
            }
            else
            {
                entryEditor.Activate();
            }

            return entryEditor;
        }

        #endregion Internal Methods

        #region Protected Methods

        #endregion Protected Methods

        #region Private Methods



        private void OnEntryEditorClosed(EntryEditorVM editor)
        {
            //TODO: This can be done better
            var foundItem = _openedEntryEditors.FirstOrDefault(item => item.Value == editor);
            _openedEntryEditors.Remove(foundItem.Key);
        }

        #endregion Private Methods
    }
}