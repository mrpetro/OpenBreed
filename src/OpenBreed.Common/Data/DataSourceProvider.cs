using EPF;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Data
{
    public class DataSourceProvider : IDisposable
    {
        #region Private Fields

        private readonly Dictionary<string, DataSourceBase> _openedDataSources = new Dictionary<string, DataSourceBase>();
        private readonly IWorkspaceMan workspaceMan;
        private readonly IVariableMan variables;
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();
        private bool disposedValue;
        private ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public DataSourceProvider(IWorkspaceMan workspaceMan, ILogger logger, IVariableMan variables)
        {
            this.workspaceMan = workspaceMan;
            this.logger = logger;
            this.variables = variables;
        }

        #endregion Public Constructors

        #region Public Methods

        public DataSourceBase GetDataSource(string name)
        {
            DataSourceBase ds = null;
            if (_openedDataSources.TryGetValue(name, out ds))
                return ds;

            var entry = workspaceMan.UnitOfWork.GetRepository<IDataSourceEntry>().GetById(name);
            if (entry == null)
                throw new Exception($"Data source error: {name}");

            ds = CreateDataSource(entry);

            return ds;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void CloseAll()
        {
            foreach (var openedArchive in _openedArchives)
            {
                openedArchive.Value.Dispose();
                logger.Verbose($"EPF Archive data source '{openedArchive.Key}' closed.");
            }

            _openedArchives.Clear();

            logger.Info($"All data sources closed.");
        }

        public void Save()
        {
            foreach (var openedArchive in _openedArchives)
            {
                openedArchive.Value.Save();
                logger.Verbose($"EPF Archive data source '{openedArchive.Key}' saved.");
            }

            logger.Info($"All data sources saved.");
        }

        #endregion Public Methods

        #region Internal Methods

        internal string ExpandVariables(string text) => variables.ExpandVariables(text);

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = IOHelper.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!_openedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToUpdate(File.Open(normalizedPath, FileMode.Open), false);
                _openedArchives.Add(normalizedPath, archive);

                logger.Verbose($"EPF Archive data source '{normalizedPath}' opened for update.");
            }

            return archive;
        }

        internal void LockDataSource(DataSourceBase ds)
        {
            _openedDataSources.Add(ds.Id, ds);
        }

        internal void ReleaseDataSource(DataSourceBase ds)
        {
            _openedDataSources.Remove(ds.Id);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CloseAll();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private DataSourceBase CreateDataSource(IDataSourceEntry dsEntry)
        {
            if (dsEntry is IFileDataSourceEntry)
                return CreateFileDataSource((IFileDataSourceEntry)dsEntry);
            else if (dsEntry is IEPFArchiveDataSourceEntry)
                return CreateEPFArchiveDataSource((IEPFArchiveDataSourceEntry)dsEntry);
            else
                throw new NotImplementedException("Unknown sourceDef");
        }

        private DataSourceBase CreateEPFArchiveDataSource(IEPFArchiveDataSourceEntry dsEntry)
        {
            return new EPFArchiveFileDataSource(this, dsEntry.Id, dsEntry.ArchivePath, dsEntry.EntryName);
        }

        private DataSourceBase CreateFileDataSource(IFileDataSourceEntry dsEntry)
        {
            return new FileDataSource(this, dsEntry.Id, dsEntry.FilePath);
        }

        #endregion Private Methods
    }
}