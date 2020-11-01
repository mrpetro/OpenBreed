using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using System;
using System.IO;

namespace OpenBreed.Editor.VM
{
    public class EditorApplication : ApplicationBase, IDisposable
    {
        #region Private Fields

        private readonly Lazy<ILogger> logger;
        private readonly Lazy<VariableMan> variables;
        private readonly Lazy<SettingsMan> settings;
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

            Settings.Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public ILogger Logger => logger.Value;

        public SettingsMan Settings => settings.Value;

        public VariableMan Variables => variables.Value;

        #endregion Public Properties

        public IUnitOfWork UnitOfWork { get; set; }

        #region Public Methods

        public IUnitOfWork OpenXmlDatabase(string databaseFilePath)
        {
            if (UnitOfWork != null)
                throw new Exception("There is already database opened.");

            UnitOfWork = XmlDatabaseMan.Open(databaseFilePath).CreateUnitOfWork();

            var directoryPath = Path.GetDirectoryName(databaseFilePath);
            var fileName = Path.GetFileName(databaseFilePath);

            Variables.RegisterVariable(typeof(string), directoryPath, "Db.Current.FolderPath");
            Variables.RegisterVariable(typeof(string), fileName, "Db.Current.FileName");

            ServiceLocator.RegisterService<IUnitOfWork>(UnitOfWork);
            ServiceLocator.RegisterService<DataProvider>(new DataProvider(UnitOfWork, Logger));

            return UnitOfWork;
        }

        public void Run() => GetInterface<EditorVM>().Run();

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

        public void CloseDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            ServiceLocator.UnregisterService<DataProvider>();
            ServiceLocator.UnregisterService<IUnitOfWork>();

            UnitOfWork = null;
        }

        #endregion Protected Methods
    }
}