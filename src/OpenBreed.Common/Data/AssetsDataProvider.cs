using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using System;

namespace OpenBreed.Common.Data
{
    public class AssetsDataProvider
    {
        #region Private Fields

        private readonly DataFormatMan formatMan;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly DataSourceProvider dataSources;

        #endregion Private Fields

        #region Public Constructors

        public AssetsDataProvider(IRepositoryProvider repositoryProvider, DataSourceProvider dataSources, DataFormatMan formatMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataSources = dataSources;
            this.formatMan = formatMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadModel(string entryId)
        {
            var assetEntry = repositoryProvider.GetRepository<IAssetEntry>().GetById(entryId);
            if (assetEntry == null)
                throw new Exception($"Asset error: {entryId}");

            var formatType = formatMan.GetFormatType(assetEntry.FormatType);

            if (formatType == null)
                throw new Exception($"Unknown format {assetEntry.FormatType}");

            var ds = dataSources.GetDataSource(assetEntry.DataSourceRef);

            if (ds == null)
                throw new Exception($"Unknown DataSourceRef {assetEntry.DataSourceRef}");

            return formatType.Load(ds, assetEntry.Parameters);
        }

        public void SaveModel(string entryId, object data)
        {
            var assetEntry = repositoryProvider.GetRepository<IAssetEntry>().GetById(entryId);
            if (assetEntry == null)
                throw new Exception($"Asset error: {entryId}");

            var formatType = formatMan.GetFormatType(assetEntry.FormatType);

            if (formatType == null)
                throw new Exception($"Unknown format {assetEntry.FormatType}");

            var ds = dataSources.GetDataSource(assetEntry.DataSourceRef);

            if (ds == null)
                throw new Exception($"Unknown DataSourceRef {assetEntry.DataSourceRef}");

            formatType.Save(ds, data, assetEntry.Parameters);
        }

        #endregion Public Methods
    }
}