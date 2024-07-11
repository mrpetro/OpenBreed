//using Microsoft.Extensions.DependencyInjection;
//using OpenBreed.Common;
//using OpenBreed.Common.Data;
//using OpenBreed.Common.Formats;
//using OpenBreed.Common.Interface.Data;
//using OpenBreed.Common.Interface.Dialog;
//using OpenBreed.Common.Interface.Logging;
//using OpenBreed.Common.Logging;
//using OpenBreed.Database.Interface;
//using OpenBreed.Database.Xml;
//using OpenBreed.Editor.Cfg.Managers;
//using OpenBreed.Editor.VM.Database;
//using OpenBreed.Editor.VM.Logging;
//using System;
//using System.IO;

//namespace OpenBreed.Editor.VM
//{
//    public class EditorApplication : IDisposable
//    {
//        #region Public Fields

//        public const string APP_NAME = "Open Breed Map Editor";

//        #endregion Public Fields

//        #region Private Fields

//        private readonly ILogger logger;
//        private readonly SettingsMan settings;
//        private readonly IWorkspaceMan workspaceMan;
//        private readonly IModelsProvider dataProvider;
//        private readonly IServiceProvider managerCollection;
//        private readonly DataSourceProvider dataSources;
//        private bool disposedValue;

//        #endregion Private Fields

//        #region Public Constructors

//        public EditorApplication(
//            IDialogProvider dialogProvider,
//            IServiceProvider managerCollection,
//            DataSourceProvider dataSources)
//        {
//            DialogProvider = dialogProvider;

//            logger = managerCollection.GetService<ILogger>();
//            settings = managerCollection.GetService<SettingsMan>();
//            dataProvider = managerCollection.GetService<IModelsProvider>();
//            workspaceMan = managerCollection.GetService<IWorkspaceMan>();

//            this.managerCollection = managerCollection;
//            this.dataSources = dataSources;

//            settings.Restore();
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public IDialogProvider DialogProvider { get; }

//        #endregion Public Properties

//        #region Public Methods

//        public void Run()
//        {
//            try
//            {
//                DialogProvider.ShowEditorView();
//            }
//            catch (Exception ex)
//            {
//                DialogProvider.ShowMessage("Critical exception: " + ex, "Open Breed Editor critial exception");
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//        public void CloseDatabase()
//        {
//            dataSources.CloseAll();

//            workspaceMan.CloseDatabase();
//        }

//        public void SaveDatabase()
//        {
//            if (workspaceMan.UnitOfWork != null)
//            {
//                ((ModelsProvider)dataProvider).Save();

//                dataSources.Save();

//                workspaceMan.SaveDatabase();
//            }

//        }

//        #endregion Public Methods

//        #region Protected Methods

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    settings.Store();
//                }

//                disposedValue = true;
//            }
//        }

//        #endregion Protected Methods
//    }
//}