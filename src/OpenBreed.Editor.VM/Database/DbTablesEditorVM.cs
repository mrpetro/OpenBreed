using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTablesEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly EditorApplication application;

        private bool isHidden;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTablesEditorVM(EditorApplication application, DbEntryFactory dbEntryFactory)
        {
            this.application = application;

            DbTableSelector = new DbTableSelectorVM(application);
            DbTableSelector.PropertyChanged += DbTableSelector_PropertyChanged;
            DbTableEditor = new DbTableEditorVM(application, dbEntryFactory);
        }

        #endregion Internal Constructors

        #region Public Properties

        public bool IsHidden
        {
            get { return isHidden; }
            set { SetProperty(ref isHidden, value); }
        }

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