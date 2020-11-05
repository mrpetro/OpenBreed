using OpenBreed.Editor.VM.Base;
using System;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTablesEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly EditorApplication application;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTablesEditorVM(EditorApplication application)
        {
            this.application = application;

            DbTableSelector = new DbTableSelectorVM(application);
            DbTableEditor = new DbTableEditorVM(application);
        }

        #endregion Internal Constructors

        #region Public Properties

        public DbTableSelectorVM DbTableSelector { get; private set; }
        public DbTableEditorVM DbTableEditor { get; private set; }

        public Action ShowingAction { get; set; }
        public Action HidingAction { get; set; }
        public Action ClosingAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool Close()
        {
            ClosingAction?.Invoke();

            return true;
        }

        public void Show()
        {
            ShowingAction?.Invoke();
        }

        public void Hide()
        {
            HidingAction?.Invoke();
        }

        #endregion Public Methods
    }
}