using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTableSelectorVM : BaseViewModel
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;
        private string currentTableName;

        #endregion Private Fields

        #region Internal Constructors

        internal DbTableSelectorVM(IWorkspaceMan workspaceMan)
        {
            this.workspaceMan = workspaceMan;
            TableNames = new BindingList<string>();
            TableNames.ListChanged += (s, a) => OnPropertyChanged(nameof(TableNames));

            UpdateWithDbTables();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string CurrentTableName
        {
            get { return currentTableName; }
            set { SetProperty(ref currentTableName, value); }
        }

        public BindingList<string> TableNames { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void Refresh()
        {
            UpdateWithDbTables();
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(TableNames):
                    CurrentTableName = TableNames.FirstOrDefault();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateWithDbTables()
        {
            TableNames.UpdateAfter(() =>
            {
                TableNames.Clear();

                foreach (var repository in workspaceMan.Repositories)
                    TableNames.Add(repository.Name);
            });
        }

        #endregion Private Methods
    }
}