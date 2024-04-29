using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Dialog;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.Cfg;
using OpenBreed.Editor.Cfg.Managers;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Linq;

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
        #region Private Fields

        private readonly DataSourceProvider dataSourceProvider;
        private readonly IModelsProvider modelsProvider;
        private readonly IServiceProvider managerCollection;
        private readonly IWorkspaceMan workspaceMan;

        private readonly SettingsMan settings;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IDialogProvider dialogProvider;
        private EditorState _state;
        private LoggerVM logger;

        private string title;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplicationVM(
            DataSourceProvider dataSourceProvider,
            IModelsProvider modelsProvider,
            IServiceProvider managerCollection,
            IWorkspaceMan workspaceMan,
            SettingsMan settings,
            DbEntryEditorFactory dbEntryEditorFactory,
            IDialogProvider dialogProvider,
            DbEditorVM dbEditor)
        {
            this.dataSourceProvider = dataSourceProvider;
            this.modelsProvider = modelsProvider;
            this.managerCollection = managerCollection;
            this.workspaceMan = workspaceMan;
            this.settings = settings;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            this.dialogProvider = dialogProvider;

            DbEditor = dbEditor;

            Title = Definitions.APP_NAME;

            ExitCommand = new Command(() => TryExit());
            OpenDatabaseCommand = new Command(() => TryOpenXmlDatabase());
            SaveDatabaseCommand = new Command(() => TrySaveDatabase());
            CloseDatabaseCommand = new Command(() => TryCloseDatabase());
            ShowOptionsCommand = new Command(() => ShowOptions());
            ShowAbtaPasswordGeneratorCommand = new Command(() => ShowAbtaPasswordGenerator());
            DbEditor.PropertyChanged += DbEditor_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ExitCommand { get; }

        public ICommand OpenDatabaseCommand { get; }

        public ICommand SaveDatabaseCommand { get; }

        public ICommand CloseDatabaseCommand { get; }

        public ICommand ShowOptionsCommand { get; }

        public ICommand ShowAbtaPasswordGeneratorCommand { get; }

        public Action<LoggerVM, bool> ToggleLoggerAction { get; set; }

        public Action ShowOptionsAction { get; set; }

        public Action ShowAbtaPasswordGeneratorAction { get; set; }

        public DbEditorVM DbEditor { get; }

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

        #endregion Public Properties

        #region Public Methods

        public void TryExit()
        {
            ExitAction?.Invoke();
        }

        public void ShowOptions()
        {
            ShowOptionsAction?.Invoke();
        }

        public void ShowAbtaPasswordGenerator()
        {
            ShowAbtaPasswordGeneratorAction?.Invoke();
        }

        public bool TryCloseDatabase()
        {
            if (TrySaveBeforeClosing())
            {
                DbEditor.CloseDatabase();

                return true;
            }
            else
                return false;
        }

        public bool TryOpenXmlDatabase()
        {
            var openFileDialog = dialogProvider.OpenFileDialog();
            openFileDialog.Title = "Select an Open Breed Editor Database file to open...";
            openFileDialog.Filter = "Open Breed Editor Database files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = XmlDatabase.DefaultDirectoryPath;
            openFileDialog.Multiselect = false;

            var answer = openFileDialog.Show();

            if (answer != DialogAnswer.OK)
            {
                return false;
            }

            string databaseFilePath = openFileDialog.FileName;

            if (!CheckCloseCurrentDatabase(databaseFilePath))
            {
                return false;
            }

            DbEditor.OpenXmlDatabase(databaseFilePath);

            return true;
        }

        public void Run()
        {
        }

        public void ToggleLogger(bool toggle)
        {
            if (logger == null)
                logger = managerCollection.GetService<LoggerVM>();

            ToggleLoggerAction?.Invoke(logger, toggle);
        }

        public void TrySaveDatabase()
        {
            DbEditor.SaveDatabase();
        }

        public bool TrySaveBeforeExiting()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (DbEditor.IsModified)
                {
                    var answer = dialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before exiting?",
                                                                               "Save database before exiting?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                    {
                        return false;
                    }
                    else if (answer == DialogAnswer.Yes)
                    {
                        DbEditor.SaveDatabase();
                    }
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
        internal bool CheckCloseCurrentDatabase(string newDatabaseFilePath)
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (IOHelper.GetNormalizedPath(newDatabaseFilePath) == IOHelper.GetNormalizedPath(DbEditor.DbName))
                {
                    //Root.Logger.Warning("Database already opened.");
                    return false;
                }

                var answer = dialogProvider.ShowMessageWithQuestion($"Another database ({DbEditor.DbName}) is already opened. Do you want to close it?",
                                                                "Close current database?",
                                                                QuestionDialogButtons.OKCancel);
                if (answer != DialogAnswer.OK)
                    return false;

                if (!TryCloseDatabase())
                    return false;
            }

            return true;
        }

        #endregion Internal Methods

        #region Private Methods

        private void DbEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DbEditor.DbName):
                    Title = $"{Definitions.APP_NAME} - {DbEditor.DbName}";
                    break;

                default:
                    break;
            }
        }

        private bool TrySaveBeforeClosing()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                if (DbEditor.IsModified)
                {
                    var answer = dialogProvider.ShowMessageWithQuestion("Current database has been modified. Do you want to save it before closing?",
                                                                               "Save database before closing?", QuestionDialogButtons.YesNoCancel);

                    if (answer == DialogAnswer.Cancel)
                    {
                        return false;
                    }
                    else if (answer == DialogAnswer.Yes)
                    {
                        DbEditor.SaveDatabase();
                    }
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