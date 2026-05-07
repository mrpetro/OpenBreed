using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {
        #region Private Fields

        private readonly DataSourceProvider dataSourceProvider;
        private readonly IWorkspaceMan workspaceMan;
        private readonly IModelsProvider modelsProvider;
        private readonly IServiceProvider managerCollection;
        private readonly Func<DbTablesEditorMainVM> dbTablesEditorInitializer;
        private readonly Func<EntityClassesTreeEditorVM> entityClassesTreeEditorInitializer;
        private bool _isDbOpened;
        private string _dbName;
        private BaseViewModel currentView;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM(
            DataSourceProvider dataSourceProvider,
            IWorkspaceMan workspaceMan,
            IModelsProvider modelsProvider,
            IServiceProvider managerCollection,
            Func<DbTablesEditorMainVM> dbTablesEditorInitializer,
            Func<EntityClassesTreeEditorVM> entityClassesTreeEditorInitializer)
        {
            this.dataSourceProvider = dataSourceProvider;
            this.workspaceMan = workspaceMan;
            this.modelsProvider = modelsProvider;
            this.managerCollection = managerCollection;
            this.dbTablesEditorInitializer = dbTablesEditorInitializer ?? throw new ArgumentNullException(nameof(dbTablesEditorInitializer));
            this.entityClassesTreeEditorInitializer = entityClassesTreeEditorInitializer ?? throw new ArgumentNullException(nameof(entityClassesTreeEditorInitializer));

            OpenTablesEditorCommand = new Command(() => OpenTablesEditor());
            OpenClassesTreeEditorCommand = new Command(() => OpenClassesTreeEditor());
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

        public BaseViewModel CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        public bool IsModified { get; internal set; }

        public ICommand OpenTablesEditorCommand { get; }

        public ICommand OpenClassesTreeEditorCommand { get; }

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

            //TablesEditor.Refresh();
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

        //public void CloseAllEditors()
        //{
        //    EntriesEditor.CloseAll();
        //    TablesEditor.Close();

        //}

        //public void ToggleDbTablesEditor(bool toggle)
        //{
        //    TablesEditor.DbTableEditor.SetModel(TablesEditor.DbTableSelector.CurrentTableName);
        //}

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods

        #region Private Methods

        private void OpenClassesTreeEditor()
        {
            CurrentView = entityClassesTreeEditorInitializer.Invoke();
        }

        private void OpenTablesEditor()
        {
            CurrentView = dbTablesEditorInitializer.Invoke();
        }

        #endregion Private Methods
    }
}