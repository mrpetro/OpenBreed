using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
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
        private readonly IDialogProvider dialogProvider;
        private readonly EditorApplicationVM editorApplicationVM;
        private readonly Func<DbTablesEditorMainVM> dbTablesEditorInitializer;
        private readonly Func<EntityClassesTreeEditorVM> entityClassesTreeEditorInitializer;
        private BaseViewModel currentView;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM(
            DataSourceProvider dataSourceProvider,
            IWorkspaceMan workspaceMan,
            IModelsProvider modelsProvider,
            IServiceProvider managerCollection,
            IDialogProvider dialogProvider,
            EditorApplicationVM editorApplicationVM,
            Func<DbTablesEditorMainVM> dbTablesEditorInitializer,
            Func<EntityClassesTreeEditorVM> entityClassesTreeEditorInitializer)
        {
            this.dataSourceProvider = dataSourceProvider;
            this.workspaceMan = workspaceMan;
            this.modelsProvider = modelsProvider;
            this.managerCollection = managerCollection;
            this.dialogProvider = dialogProvider ?? throw new ArgumentNullException(nameof(dialogProvider));
            this.editorApplicationVM = editorApplicationVM ?? throw new ArgumentNullException(nameof(editorApplicationVM));
            this.dbTablesEditorInitializer = dbTablesEditorInitializer ?? throw new ArgumentNullException(nameof(dbTablesEditorInitializer));
            this.entityClassesTreeEditorInitializer = entityClassesTreeEditorInitializer ?? throw new ArgumentNullException(nameof(entityClassesTreeEditorInitializer));

            OpenTablesEditorCommand = new Command(() => OpenTablesEditor());
            OpenClassesTreeEditorCommand = new Command(() => OpenClassesTreeEditor());
            SaveDatabaseCommand = new Command(() => TrySaveDatabase());
            CloseDatabaseCommand = new Command(() => TryCloseDatabase());

            SetupMenus();
        }

        private void CleanupMenus()
        {
            var fileMenu = editorApplicationVM.Menus.First(item => item.Header == "File");

            var openDatabaseMenuItem = fileMenu.Menus.First(item => item.Header == "Open Database...");

            openDatabaseMenuItem.IsEnabled = true;



            var closeMenuItem = fileMenu.Menus.First(item => item.Header == "Close Database");

            fileMenu.Menus.Remove(closeMenuItem);

            var saveMenuItem = fileMenu.Menus.First(item => item.Header == "Save Database");

            fileMenu.Menus.Remove(saveMenuItem);

            var viewMenu = editorApplicationVM.Menus.First(item => item.Header == "View");

            var tablesEditorMenuItem = viewMenu.Menus.First(item => item.Header == "Tables editor");
            viewMenu.Menus.Remove(tablesEditorMenuItem);

            var classesTreeEditorMenuItem = viewMenu.Menus.First(item => item.Header == "Classes tree editor");
            viewMenu.Menus.Remove(classesTreeEditorMenuItem);
        }

        private void SetupMenus()
        {
            var fileMenu = editorApplicationVM.Menus.First(item => item.Header == "File");

            var openDatabaseMenuItem =  fileMenu.Menus.First(item => item.Header == "Open Database...");

            openDatabaseMenuItem.IsEnabled = false;

            var insertIndex = fileMenu.Menus.IndexOf(openDatabaseMenuItem);

            fileMenu.Menus.Insert(insertIndex + 1, new MenuItemVM()
            {
                Header = "Close Database",
                Command = CloseDatabaseCommand,
                IsEnabled = true
            });

            fileMenu.Menus.Insert(insertIndex + 1, new MenuItemVM()
            {
                Header = "Save Database",
                Command = SaveDatabaseCommand,
                IsEnabled = true
            });

            var viewMenu = editorApplicationVM.Menus.First(item => item.Header == "View");

            viewMenu.Menus.Add(new MenuItemVM()
            {
                Header = "Tables editor",
                Command = OpenTablesEditorCommand,
                IsEnabled = true
            });

            viewMenu.Menus.Add(new MenuItemVM()
            {
                Header = "Classes tree editor",
                Command = OpenClassesTreeEditorCommand,
                IsEnabled = true
            });
        }

        #endregion Public Constructors

        #region Public Properties

        public BaseViewModel CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        public ICommand OpenTablesEditorCommand { get; }

        public ICommand OpenClassesTreeEditorCommand { get; }

        public ICommand SaveDatabaseCommand { get; }

        public bool IsModified { get; internal set; }

        public ICommand CloseDatabaseCommand { get; }

        #endregion Public Properties

        #region Public Methods

        public bool TryCloseDatabase()
        {
            if (TrySaveBeforeClosing())
            {
                CloseDatabase();

                editorApplicationVM.DbName = null;
                editorApplicationVM.IsDbOpened = false;
                editorApplicationVM.CurrentView = null;

                CleanupMenus();

                return true;
            }
            else
                return false;
        }

        public void CloseDatabase()
        {
            dataSourceProvider.CloseAll();
            workspaceMan.CloseDatabase();
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

        public bool TrySaveBeforeExiting()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (IsModified)
                {
                    var answer = dialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                    {
                        return false;
                    }
                    else if (answer == DialogAnswer.Yes)
                    {
                        SaveDatabase();
                    }
                }
            }

            return true;
        }

        public void TrySaveDatabase()
        {
            SaveDatabase();
        }

        #endregion Public Methods

        #region Private Methods

        private bool TrySaveBeforeClosing()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (IsModified)
                {
                    var answer = dialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                               "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                    {
                        return false;
                    }
                    else if (answer == DialogAnswer.Yes)
                    {
                        SaveDatabase();
                    }
                }
            }

            return true;
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