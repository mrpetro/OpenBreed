using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTablesEditorVM : BaseViewModel
    {
        #region Private Fields

        #endregion Private Fields

        #region Internal Constructors

        public DbTablesEditorVM(IWorkspaceMan workspaceMan,
                                  DbEntryFactory dbEntryFactory)
        {
            DbTableSelector = new DbTableSelectorVM(workspaceMan);
            DbTableSelector.PropertyChanged += DbTableSelector_PropertyChanged;
            DbTableEditor = new DbTableEditorVM(workspaceMan, dbEntryFactory);
        }

        #endregion Internal Constructors

        #region Public Properties

        public DbTableSelectorVM DbTableSelector { get; private set; }

        public DbTableEditorVM DbTableEditor { get; private set; }

        public Action CloseAction { get; set; }

        #endregion Public Properties

        #region Internal Properties

        internal EntryEditorOpener EntryEditorOpener
        {
            get => DbTableEditor.EntryEditorOpener;

            set => DbTableEditor.EntryEditorOpener = value;
        }

        #endregion Internal Properties

        #region Public Methods

        public bool Close()
        {
            CloseAction?.Invoke();

            return true;
        }

        internal void Refresh()
        {
            DbTableSelector.Refresh();
            DbTableEditor.Refresh();
        }

        #endregion Public Methods

        #region Private Methods

        private void DbTableSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tableSelector = sender as DbTableSelectorVM;

            switch (e.PropertyName)
            {
                case nameof(tableSelector.CurrentTableName):
                    OnTableChanged(tableSelector.CurrentTableName);
                    break;

                default:
                    break;
            }
        }

        private void OnTableChanged(string tableName)
        {
            if (tableName != null)
                DbTableEditor.SetModel(tableName);
            else
                DbTableEditor.SetNoModel();
        }

        #endregion Private Methods
    }
}