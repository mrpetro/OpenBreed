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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Input;
using System.Xml.Linq;

namespace OpenBreed.Editor.VM
{
    public enum EditorState
    {
        NoDatabase,
        Exiting,
        DatabaseOpened
    }

    public class EditorApplicationVM : BaseViewModel
    {
        #region Private Fields

        private const string NoDatabaseOpenedInfo = "No database opened.";
        private readonly DataSourceProvider dataSourceProvider;
        private readonly IModelsProvider modelsProvider;
        private readonly IServiceProvider managerCollection;
        private readonly IWorkspaceMan workspaceMan;

        private readonly SettingsMan settings;
        private readonly DbEntryEditorFactory dbEntryEditorFactory;
        private readonly IDialogProvider dialogProvider;
        private EditorState _state;
        private string title;
        private bool _isDbOpened;
        private string _dbName;
        private readonly Func<DbEditorVM> dbEditorInitializer;
        private BaseViewModel currentView;

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
            Func<DbEditorVM> dbEditorInitializer)
        {
            this.dataSourceProvider = dataSourceProvider;
            this.modelsProvider = modelsProvider;
            this.managerCollection = managerCollection;
            this.workspaceMan = workspaceMan;
            this.settings = settings;
            this.dbEntryEditorFactory = dbEntryEditorFactory;
            this.dialogProvider = dialogProvider;
            this.dbEditorInitializer = dbEditorInitializer;

            Logger = managerCollection.GetService<LoggerVM>();

            ExitCommand = new Command(() => TryExit());
            OpenDatabaseCommand = new Command(() => TryOpenXmlDatabase());
            ShowOptionsCommand = new Command(() => ShowOptions());

            ShowAbtaPasswordGeneratorCommand = new Command(() => ShowAbtaPasswordGenerator());

            UpdateTitle();
            RebuildMenus();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ExitCommand { get; }

        public ICommand OpenDatabaseCommand { get; }

        public ICommand ShowOptionsCommand { get; }

        public ICommand ToggleTablesEditorCommand { get; }

        public ICommand ShowAbtaPasswordGeneratorCommand { get; }

        public Action ShowOptionsAction { get; set; }

        public Action ShowAbtaPasswordGeneratorAction { get; set; }

        public LoggerVM Logger { get; }

        public ObservableCollection<MenuItemVM> Menus { get; } = new();

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

            //if (!CheckCloseCurrentDatabase(databaseFilePath))
            //{
            //    return false;
            //}

            workspaceMan.OpenXmlDatabase(databaseFilePath);

            DbName = workspaceMan.UnitOfWork.Name;
            IsDbOpened = true;

            CurrentView = dbEditorInitializer.Invoke();

            return true;
        }

        public void Run()
        {
        }

        public void TryRunABTAGame()
        {
            Other.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Internal Methods

        ///// <summary>
        ///// This checks if database is opened already,
        ///// If it is then it asks of it can be closed
        ///// </summary>
        ///// <returns>True if no database was opened or if previous one was closed, false otherwise</returns>
        //internal bool CheckCloseCurrentDatabase(string newDatabaseFilePath)
        //{
        //    if (workspaceMan.UnitOfWork != null)
        //    {
        //        if (IOHelper.GetNormalizedPath(newDatabaseFilePath) == IOHelper.GetNormalizedPath(DbName))
        //        {
        //            //Root.Logger.Warning("Database already opened.");
        //            return false;
        //        }

        //        var answer = dialogProvider.ShowMessageWithQuestion($"Another database ({DbName}) is already opened. Do you want to close it?",
        //                                                        "Close current database?",
        //                                                        QuestionDialogButtons.OKCancel);
        //        if (answer != DialogAnswer.OK)
        //            return false;

        //        if (!TryCloseDatabase())
        //            return false;
        //    }

        //    return true;
        //}

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(State):
                    RebuildMenus();
                    break;

                case nameof(DbName):
                    UpdateTitle();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RebuildMenus()
        {
            Menus.Clear();

            Menus.Add(BuildFileMenu());
            Menus.Add(BuildViewMenu());
            Menus.Add(BuildToolsMenu());

            //switch (State)
            //{
            //    case EditorState.NoDatabase:
            //        Menus.Add(BuildLoginMenu());
            //        break;

            //    case EditorState.DatabaseOpened:
            //        Menus.Add(BuildFileMenu());
            //        Menus.Add(BuildToolsMenu());
            //        break;
            //}
        }

        private MenuItemVM BuildFileMenu()
        {
            var menu = new MenuItemVM()
            {
                Header = "File",
                IsEnabled = true
            };

            menu.Menus.Add(new MenuItemVM()
            {
                Header = "Open Database...",
                Command = OpenDatabaseCommand,
            });

            menu.Menus.Add(new MenuItemVM()
            {
                IsSeparator = true
            });

            menu.Menus.Add(new MenuItemVM()
            {
                Header = "Exit",
                Command = ExitCommand,
                IsEnabled = true
            });

            return menu;
        }

        private MenuItemVM BuildViewMenu()
        {
            var menu = new MenuItemVM()
            {
                Header = "View",
                IsEnabled = true
            };


            return menu;
        }

        private MenuItemVM BuildToolsMenu()
        {
            var menu = new MenuItemVM()
            {
                Header = "Tools",
                IsEnabled = true
            };

            //menu.Children.Add(new MenuItemViewModel()
            //{
            //    Header = "EPF pack/unpack...",
            //    Command = RunEpfManagerCommand,
            //    IsEnabled = true
            //});

            menu.Menus.Add(new MenuItemVM()
            {
                Header = "Options...",
                Command = ShowOptionsCommand,
                IsEnabled = true
            });

            //menu.Children.Add(new MenuItemViewModel()
            //{
            //    Header = "Log console",
            //    Command = ShowLogConsoleCommand,
            //    IsEnabled = true
            //});

            menu.Menus.Add(new MenuItemVM()
            {
                Header = "ABTA Game passwords....",
                Command = ShowAbtaPasswordGeneratorCommand,
                IsEnabled = true
            });

            return menu;
        }

        private MenuItemVM BuildRunMenu()
        {
            var menu = new MenuItemVM()
            {
                Header = "Run game",
                IsEnabled = true
            };

            //menu.Children.Add(new MenuItemViewModel()
            //{
            //    Header = "AB: Special Edition",
            //    Command = RunAbseGameCommand,
            //    IsEnabled = true
            //});

            //menu.Children.Add(new MenuItemViewModel()
            //{
            //    Header = "AB: The Horror Continues",
            //    Command = RunAbhcGameCommand,
            //    IsEnabled = true
            //});

            //menu.Children.Add(new MenuItemViewModel()
            //{
            //    Header = "AB: Tower Assault",
            //    Command = RunAbtaGameCommand,
            //    IsEnabled = true
            //});

            return menu;
        }

        private void UpdateTitle()
        {
            if (string.IsNullOrEmpty(DbName))
            {
                Title = $"{Definitions.APP_NAME} - {NoDatabaseOpenedInfo}";
                return;
            }

            Title = $"{Definitions.APP_NAME} - {DbName}";
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