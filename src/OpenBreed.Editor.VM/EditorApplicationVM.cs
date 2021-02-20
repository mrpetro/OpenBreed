using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace OpenBreed.Editor.VM
{
    public enum EditorState
    {
        Active,
        Exiting,
        Exited
    }

    public class EditorApplicationVM : BaseViewModel
    {
        #region Public Fields

        public EditorApplication application;

        #endregion Public Fields

        #region Private Fields

        private readonly IManagerCollection managerCollection;
        private readonly IWorkspaceMan workspaceMan;

        private readonly SettingsMan settings;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IDialogProvider dialogProvider;
        private readonly Lazy<DbEditorVM> lazyDbEditor;
        private EditorState _state;
        private LoggerVM logger;

        private string title;

        private string dbName;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplicationVM(EditorApplication application, IManagerCollection managerCollection, IWorkspaceMan workspaceMan, SettingsMan settings, DbEntryEditorFactory dbEntryEditorFactory, IDialogProvider dialogProvider)
        {
            this.application = application;
            this.managerCollection = managerCollection;
            this.workspaceMan = workspaceMan;
            this.settings = settings;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            this.dialogProvider = dialogProvider;

            lazyDbEditor = new Lazy<DbEditorVM>(() => managerCollection.GetManager<DbEditorVM>());

            MenuItems = new BindingList<MenuItemVM>();

            Title = EditorApplication.APP_NAME;
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<MenuItemVM> MenuItems { get; }

        public Action<LoggerVM, bool> ToggleLoggerAction { get; set; }

        public Action<SettingsMan> ShowOptionsAction { get; set; }

        public DbEditorVM DbEditor => lazyDbEditor.Value;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public Action ExitAction { get; set; }

        public string DbName
        {
            get { return dbName; }
            set { SetProperty(ref dbName, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void TryExit()
        {
            ExitAction?.Invoke();
        }

        public void ShowOptions()
        {
            ShowOptionsAction?.Invoke(settings);
        }

        public bool TryCloseDatabase(DbEditorVM dbEditorVM)
        {
            if (TrySaveBeforeClosing(dbEditorVM))
            {
                DbName = null;
                application.CloseDatabase();

                return true;
            }
            else
                return false;
        }

        public bool TryOpenXmlDatabase(DbEditorVM dbEditorVM)
        {
            var openFileDialog = dialogProvider.OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = XmlDatabase.DefaultDirectoryPath;
            openFileDialog.Multiselect = false;

            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
                return false;

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(databaseFilePath, dbEditorVM))
                return false;

            application.OpenXmlDatabase(databaseFilePath);

            DbName = workspaceMan.UnitOfWork.Name;

            return true;
        }

        public void Run()
        {
        }

        public void ToggleLogger(bool toggle)
        {
            if (logger == null)
                logger = managerCollection.GetManager<LoggerVM>();

            ToggleLoggerAction?.Invoke(logger, toggle);
        }

        public void TrySaveDatabase()
        {
            application.SaveDatabase();
        }

        public bool TrySaveBeforeExiting(DbEditorVM dbEditorVM)
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (dbEditorVM.IsModified)
                {
                    var answer = dialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                        return false;
                    else if (answer == DialogAnswer.Yes)
                        application.SaveDatabase();
                }
            }

            return true;
        }

        public void TryRunABTAGame()
        {
            Other.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// This checks if database is opened already,
        /// If it is then it asks of it can be closed
        /// </summary>
        /// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        internal bool CheckCloseCurrentDatabase(string newDatabaseFilePath, DbEditorVM dbEditorVM)
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (IOHelper.GetNormalizedPath(newDatabaseFilePath) == IOHelper.GetNormalizedPath(DbName))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = dialogProvider.ShowMessageWithQuestion($"Another database ({DbName}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!TryCloseDatabase(dbEditorVM))
                    return false;
            }

            return true;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(DbName):
                    Title = $"{EditorApplication.APP_NAME} - {DbName}";
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool TrySaveBeforeClosing(DbEditorVM dbEditorVM)
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (dbEditorVM.IsModified)
                {
                    var answer = application.DialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                               "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                        return false;
                    else if (answer == DialogAnswer.Yes)
                        application.SaveDatabase();
                }
            }

            return true;
        }

        private void RunABTAGame()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = settings.Cfg.Options.ABTA.GameRunFilePath;
            proc.StartInfo.Arguments = settings.Cfg.Options.ABTA.GameRunFileArgs;
            proc.StartInfo.WorkingDirectory = settings.Cfg.Options.ABTA.GameFolderPath;
            proc.Start();
        }

        #endregion Private Methods
    }
}