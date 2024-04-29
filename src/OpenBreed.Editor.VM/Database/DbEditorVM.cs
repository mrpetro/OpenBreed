using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly Dictionary<string, EntryEditorVM> _openedEntryEditors = new Dictionary<string, EntryEditorVM>();
        private readonly DataSourceProvider dataSourceProvider;
        private readonly IWorkspaceMan workspaceMan;
        private readonly IModelsProvider modelsProvider;
        private readonly IServiceProvider managerCollection;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IRepositoryProvider repositoryProvider;
        private bool _isDbOpened;
        private string _dbName;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM(
            DataSourceProvider dataSourceProvider,
            IWorkspaceMan workspaceMan,
            IModelsProvider modelsProvider,
            IServiceProvider managerCollection,
            DbEntryEditorFactory dbEntryEditorFactory,
            IRepositoryProvider repositoryProvider,
            DbTablesEditorVM tablesEditor,
            DbEntriesEditorVM entriesEditor)
        {
            this.dataSourceProvider = dataSourceProvider;
            this.workspaceMan = workspaceMan;
            this.modelsProvider = modelsProvider;
            this.managerCollection = managerCollection;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            this.repositoryProvider = repositoryProvider;

            EntriesEditor = entriesEditor;
            TablesEditor = tablesEditor;

            TablesEditor.EntryEditorOpener = OpenEntryEditor;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsDbOpened
        {
            get { return _isDbOpened; }
            set { SetProperty(ref _isDbOpened, value); }
        }

        public string DbName
        {
            get { return _dbName; }
            set { SetProperty(ref _dbName, value); }
        }

        public Action<EntryEditorVM> EntryEditorOpeningAction { get; set; }

        public bool IsModified { get; internal set; }

        public DbTablesEditorVM TablesEditor { get; }

        public DbEntriesEditorVM EntriesEditor { get; }

        #endregion Public Properties

        #region Public Methods

        public void CloseDatabase()
        {
            dataSourceProvider.CloseAll();
            workspaceMan.CloseDatabase();

            DbName = null;
            IsDbOpened = false;
        }

        public void OpenXmlDatabase(string databaseFilePath)
        {
            workspaceMan.OpenXmlDatabase(databaseFilePath);

            DbName = workspaceMan.UnitOfWork.Name;
            IsDbOpened = true;

            TablesEditor.Refresh();
        }

        public void SaveDatabase()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                ((ModelsProvider)modelsProvider).Save();
                dataSourceProvider.Save();

                workspaceMan.SaveDatabase();
            }
        }

        public void CloseAllEditors()
        {
            var toClose = _openedEntryEditors.Values.ToArray();

            foreach (var entryEditor in toClose)
                entryEditor.Close();

            CloseDbTablesEditor();
        }

        public void CloseDbTablesEditor()
        {
            TablesEditor.Close();
        }

        public void ToggleDbTablesEditor(bool toggle)
        {
            TablesEditor.DbTableEditor.SetModel(TablesEditor.DbTableSelector.CurrentTableName);
        }

        #endregion Public Methods

        #region Internal Methods

        internal EntryEditorVM OpenEntryEditor(string tableName, string entryId)
        {
            string entryEditorKey = $"{tableName}#{entryId}";

            EntryEditorVM entryEditor = null;
            if (!_openedEntryEditors.TryGetValue(entryEditorKey, out entryEditor))
            {
                var repository = repositoryProvider.GetRepository(tableName);

                var entry = repository.Find(entryId);

                entryEditor = dbEntryEditorFactory.Create(repository, entry);

                AddEntryEditor(entryEditorKey, entryEditor);

                entryEditor.ClosedAction = () => OnEntryEditorClosed(entryEditor);
                entryEditor.EditEntry(entryId);
                EntryEditorOpeningAction?.Invoke(entryEditor);
            }
            else
            {
                EntriesEditor.ActivateEditor(entryEditor);
            }

            return entryEditor;
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddEntryEditor(string editorKey, EntryEditorVM entryEditor)
        {
            _openedEntryEditors.Add(editorKey, entryEditor);

            EntriesEditor.AddEditor(editorKey, entryEditor);
        }

        private void OnEntryEditorClosed(EntryEditorVM editor)
        {
            //TODO: This can be done better
            var foundItem = _openedEntryEditors.FirstOrDefault(item => item.Value == editor);
            _openedEntryEditors.Remove(foundItem.Key);
        }

        #endregion Private Methods
    }
}