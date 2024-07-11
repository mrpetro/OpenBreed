using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
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

        private readonly IRepositoryProvider repositoryProvider;
        private readonly DataSourceProvider dataSources;

        #endregion Private Fields

        #region Public Constructors

        public AssetsDataProvider(
            IRepositoryProvider repositoryProvider,
            DataSourceProvider dataSources,
            IEnumerable<IAssetDataHandler> handlers)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataSources = dataSources;
            this.handlers = handlers.ToDictionary(item => item.EntryType, item => item);
        }

        #endregion Public Constructors

        #region Public Methods

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

        public void SaveModel(Type entryType, string entryId, object model)
        {
            var dbEntry = repositoryProvider.GetRepository(entryType).Find(entryId);

            if (dbEntry is null)
            {
                throw new Exception($"Entry '{entryId}' of type '{entryType}' not found.");
            }

            if (!handlers.TryGetValue(entryType, out IAssetDataHandler handler))
            {
                throw new Exception($"No save handler for '{entryType}'.");
            }

            handler.Save(dbEntry, model);
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

        #endregion Public Methods
    }
}