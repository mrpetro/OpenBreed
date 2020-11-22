using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Logging;
using System;
using System.IO;

namespace OpenBreed.Editor.VM
{
    public class EditorApplication : ApplicationBase, IDisposable
    {
        #region Public Fields

        public const string APP_NAME = "Open Breed Map Editor";

        #endregion Public Fields

        #region Private Fields

        private readonly Lazy<ILogger> logger;
        private readonly Lazy<VariableMan> variables;
        private readonly Lazy<SettingsMan> settings;
        private readonly Lazy<IDialogProvider> dialogProvider;
        private readonly XmlDatabaseMan databaseMan;

        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplication()
        {
            RegisterInterface<ILogger>(() => new DefaultLogger());
            RegisterInterface<VariableMan>(() => new VariableMan(this));
            RegisterInterface<SettingsMan>(() => new SettingsMan(this));

            logger = new Lazy<ILogger>(GetInterface<ILogger>);
            variables = new Lazy<VariableMan>(GetInterface<VariableMan>);
            settings = new Lazy<SettingsMan>(GetInterface<SettingsMan>);
            dialogProvider = new Lazy<IDialogProvider>(GetInterface<IDialogProvider>);
            databaseMan = new XmlDatabaseMan(Variables);

            Settings.Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public ILogger Logger => logger.Value;

        public SettingsMan Settings => settings.Value;

        public VariableMan Variables => variables.Value;

        public IUnitOfWork UnitOfWork { get; private set; }
        public DataProvider DataProvider { get; private set; }
        public IDialogProvider DialogProvider => dialogProvider.Value;

        #endregion Public Properties

        #region Public Methods

        public void OpenXmlDatabase(string databaseFilePath)
        {
            if (UnitOfWork != null)
                throw new Exception("There is already database opened.");

            databaseMan.Open(databaseFilePath);
            UnitOfWork = databaseMan.CreateUnitOfWork();

            DataProvider = new DataProvider(UnitOfWork, Logger, Variables);

            Logger.Info($"Database '{UnitOfWork.Name}' opened.");
        }

        public void Run()
        {
            try
            {
                DialogProvider.ShowEditorView();
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Open Breed Editor critial exception");
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void CloseDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            var databaseName = UnitOfWork.Name;

            DataProvider.Close();
            databaseMan.Close();

            UnitOfWork = null;
            DataProvider = null;

            Logger.Info($"Database '{databaseName}' closed.");
        }

        public void SaveDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            DataProvider.Save();

            Logger.Info($"Database '{UnitOfWork.Name}' saved.");
        }

        public LoggerVM CreateLoggerVm()
        {
            return new LoggerVM(Logger);
        }

        public DbEditorVM CreateDbEditorVm(IUnitOfWork unitOfWork)
        {
            return new DbEditorVM(this);
        }

        public DbTablesEditorVM CreateDbTablesEditorVm()
        {
            return new DbTablesEditorVM(this);
        }

        public EditorApplicationVM CreateEditorApplicationVm()
        {
            return new EditorApplicationVM(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Settings.Store();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}