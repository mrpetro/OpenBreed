using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
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

        private readonly ILogger logger;
        private readonly SettingsMan settings;
        private readonly Lazy<IDialogProvider> dialogProvider;
        private readonly XmlDatabaseMan databaseMan;
        private readonly IManagerCollection managerCollection;
        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplication(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;

            logger = managerCollection.GetManager<ILogger>();
            settings = managerCollection.GetManager<SettingsMan>();
            databaseMan = managerCollection.GetManager<XmlDatabaseMan>();       
            dialogProvider = new Lazy<IDialogProvider>(() => managerCollection.GetManager<IDialogProvider>());

            settings.Restore();
        }

        #endregion Public Constructors

        #region Public Properties

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

            DataProvider = new DataProvider(UnitOfWork, managerCollection.GetManager<ILogger>(), 
                                                        managerCollection.GetManager<VariableMan>(),
                                                        managerCollection.GetManager<DataFormatMan>());

            logger.Info($"Database '{UnitOfWork.Name}' opened.");
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

            logger.Info($"Database '{databaseName}' closed.");
        }

        public void SaveDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            DataProvider.Save();

            logger.Info($"Database '{UnitOfWork.Name}' saved.");
        }

        public LoggerVM CreateLoggerVm()
        {
            return new LoggerVM(logger);
        }

        public DbEditorVM CreateDbEditorVm(IUnitOfWork unitOfWork)
        {
            return new DbEditorVM(this, managerCollection.GetManager<DbEntryEditorFactory>());
        }

        public DbTablesEditorVM CreateDbTablesEditorVm()
        {
            return new DbTablesEditorVM(this, managerCollection.GetManager<DbEntryFactory>());
        }

        public EditorApplicationVM CreateEditorApplicationVm()
        {
            return new EditorApplicationVM(this, settings, managerCollection.GetManager<DbEntryEditorFactory>(), DialogProvider);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    settings.Store();
                }

                disposedValue = true;
            }
        }

        public TManager GetManager<TManager>()
        {
            throw new NotImplementedException();
        }

        public void AddSingleton<TInterface>(Func<object> initializer)
        {
            throw new NotImplementedException();
        }

        public void AddSingleton<TInterface>(TInterface instance)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}