using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;

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