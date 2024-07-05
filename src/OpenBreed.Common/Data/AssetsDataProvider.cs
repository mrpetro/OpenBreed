using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBreed.Common.Data
{
    public interface IAssetDataHandler
    {
        #region Public Properties

        Type EntryType { get; }

        #endregion Public Properties

        #region Public Methods

        object Load(IDbEntry dbEntry);

        void Save(IDbEntry dbEntry, object model);

        #endregion Public Methods
    }

    public abstract class AssetDataHandlerBase<TDbEntry> : IAssetDataHandler where TDbEntry : IDbEntry
    {
        #region Public Properties

        public Type EntryType => typeof(TDbEntry);

        #endregion Public Properties

        #region Public Methods

        public void Save(IDbEntry dbEntry, object model)
        {
            Save((TDbEntry)dbEntry, model);
        }

        public object Load(IDbEntry dbEntry)
        {
            return Load((TDbEntry)dbEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void Save(TDbEntry dbEntry, object model);

        protected abstract object Load(TDbEntry dbEntry);

        #endregion Protected Methods
    }

    public class AssetsDataProvider
    {
        #region Private Fields

        private readonly Dictionary<Type, IAssetDataHandler> handlers;

        private readonly DataFormatMan formatMan;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly DataSourceProvider dataSources;

        #endregion Private Fields

        #region Public Constructors

        public AssetsDataProvider(
            IRepositoryProvider repositoryProvider,
            DataSourceProvider dataSources,
            DataFormatMan formatMan,
            IEnumerable<IAssetDataHandler> handlers)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataSources = dataSources;
            this.formatMan = formatMan;

            this.handlers = handlers.ToDictionary(item => item.EntryType, item => item);
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadModel(string dataSourceRef, string formatType, List<FormatParameter> parameters)
        {
            var ds = dataSources.GetDataSource(dataSourceRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dataSourceRef}'.");
            }

            var formatTypeHandler = formatMan.GetFormatType(formatType);

            if (formatTypeHandler is null)
            {
                throw new Exception($"Unknown format '{formatType}'.");
            }

            return formatTypeHandler.Load(ds, parameters);
        }

        public object LoadModel<TDbEntry>(TDbEntry dbEntry) where TDbEntry : IDbEntry
        {
            if (dbEntry is null)
            {
                throw new ArgumentNullException(nameof(dbEntry));
            }

            if (!handlers.TryGetValue(typeof(TDbEntry), out IAssetDataHandler handler))
            {
                throw new Exception($"No load handler for '{typeof(TDbEntry)}'.");
            }

            return handler.Load(dbEntry);
        }

        public object LoadModel<TDbEntry>(string entryId) where TDbEntry : IDbEntry
        {
            var dbEntry = repositoryProvider.GetRepository<TDbEntry>().GetById(entryId);

            if (dbEntry is null)
            {
                throw new Exception($"Entry '{entryId}' of type '{nameof(TDbEntry)}' not found.");
            }

            return LoadModel<TDbEntry>(dbEntry);
        }

        public object LoadModel(string entryId)
        {
            var assetEntry = repositoryProvider.GetRepository<IDbAsset>().GetById(entryId);

            if (assetEntry is null)
            {
                throw new Exception($"Asset entry '{entryId}' not found.");
            }

            return LoadModel(assetEntry.DataSourceRef, assetEntry.FormatType, assetEntry.Parameters);
        }

        public void SaveModel(string dataSourceRef, string formatType, List<FormatParameter> parameters, object data)
        {
            var ds = dataSources.GetDataSource(dataSourceRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dataSourceRef}'.");
            }

            var formatTypeHandler = formatMan.GetFormatType(formatType);

            if (formatTypeHandler is null)
            {
                throw new Exception($"Unknown format '{formatType}'.");
            }

            formatTypeHandler.Save(ds, data, parameters);
        }

        public void SaveModel<TDbEntry>(TDbEntry dbEntry, object model) where TDbEntry : IDbEntry
        {
            if (dbEntry is null)
            {
                throw new ArgumentNullException(nameof(dbEntry));
            }

            if (!handlers.TryGetValue(typeof(TDbEntry), out IAssetDataHandler handler))
            {
                throw new Exception($"No save handler for '{typeof(TDbEntry)}'.");
            }

            handler.Save(dbEntry, model);
        }

        public void SaveModel(string entryId, object data)
        {
            var assetEntry = repositoryProvider.GetRepository<IDbAsset>().GetById(entryId);

            if (assetEntry is null)
            {
                throw new Exception($"Asset entry '{entryId}' not found.");
            }

            SaveModel(assetEntry.DataSourceRef, assetEntry.FormatType, assetEntry.Parameters, data);
        }

        #endregion Public Methods
    }
}