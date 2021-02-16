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
        private readonly IWorkspaceMan workspaceMan;
        private readonly IDataProvider dataProvider;
        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public EditorApplication(IManagerCollection managerCollection) : base(managerCollection)
        {

            logger = managerCollection.GetManager<ILogger>();

            settings = managerCollection.GetManager<SettingsMan>();
            dataProvider = managerCollection.GetManager<IDataProvider>();
            databaseMan = managerCollection.GetManager<XmlDatabaseMan>();
            workspaceMan = managerCollection.GetManager<IWorkspaceMan>();
            dialogProvider = new Lazy<IDialogProvider>(() => managerCollection.GetManager<IDialogProvider>());
            //DialogProvider = managerCollection.GetManager<IDialogProvider>();

            settings.Restore();
        }

        #endregion Public Constructors

        #region Public Properties

        public IDialogProvider DialogProvider => dialogProvider.Value;

        #endregion Public Properties

        #region Public Methods

        public void OpenXmlDatabase(string databaseFilePath)
        {
            workspaceMan.OpenXmlDatabase(databaseFilePath);
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
            //TODO: Lazy thing here, fix that at some point
            ((DataProvider)dataProvider).Close();
            workspaceMan.CloseDatabase();
        }

        public void SaveDatabase()
        {
            if (workspaceMan.UnitOfWork != null)
            {
                ((DataProvider)dataProvider).Save();
                workspaceMan.SaveDatabase();
            }

        }

        public LoggerVM CreateLoggerVm()
        {
            return new LoggerVM(logger);
        }

        public DbEditorVM CreateDbEditorVm()
        {
            return new DbEditorVM(this, managerCollection, managerCollection.GetManager<DbEntryEditorFactory>(), workspaceMan, (DataProvider)dataProvider, DialogProvider);
        }

        public DbTablesEditorVM CreateDbTablesEditorVm()
        {
            return new DbTablesEditorVM(workspaceMan, managerCollection.GetManager<DbEntryFactory>());
        }

        public EditorApplicationVM CreateEditorApplicationVm()
        {
            return new EditorApplicationVM(this, managerCollection, workspaceMan, (DataProvider)dataProvider , settings, managerCollection.GetManager<DbEntryEditorFactory>(), DialogProvider);
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