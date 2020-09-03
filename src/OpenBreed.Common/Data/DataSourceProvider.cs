using EPF;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Helpers;
using OpenBreed.Database.Interface.Items.DataSources;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Data
{
    public delegate string ExpandVariablesDelegate(string text);

    public class DataSourceProvider
    {
        #region Private Fields

        private readonly Dictionary<string, DataSourceBase> _openedDataSources = new Dictionary<string, DataSourceBase>();
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();

        #endregion Private Fields

        #region Public Constructors

        public DataSourceProvider(DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public static ExpandVariablesDelegate ExpandGlobalVariables { get; set; }
        public DataProvider DataProvider { get; }

        #endregion Public Properties

        #region Public Methods

        public DataSourceBase GetDataSource(string name)
        {
            DataSourceBase ds = null;
            if (_openedDataSources.TryGetValue(name, out ds))
                return ds;

            var entry = DataProvider.UnitOfWork.GetRepository<IDataSourceEntry>().GetById(name);
            if (entry == null)
                throw new Exception($"Data source error: {name}");

            ds = CreateDataSource(entry);

            return ds;
        }

        #endregion Public Methods

        #region Internal Methods

        internal string ExpandVariables(string text)
        {
            var result = ExpandGlobalVariables(text);



            return result;
        }

        internal void CloseAll()
        {
            Save();

            foreach (var openedArchive in _openedArchives)
                openedArchive.Value.Dispose();

            _openedArchives.Clear();
        }

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = IOHelper.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!_openedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToUpdate(File.Open(normalizedPath, FileMode.Open), true);
                _openedArchives.Add(normalizedPath, archive);
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

        internal void Save()
        {
            foreach (var openedArchive in _openedArchives)
                openedArchive.Value.Save();
        }

        #endregion Internal Methods

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