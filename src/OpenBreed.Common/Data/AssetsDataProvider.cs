using OpenBreed.Common.Assets;
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

        private readonly Dictionary<string, AssetBase> _openedAssets = new Dictionary<string, AssetBase>();
        private readonly DataFormatMan formatMan;
        private readonly IUnitOfWork unitOfWork;
        private readonly DataSourceProvider dataSources;

        #endregion Private Fields

        #region Public Constructors

        public AssetsDataProvider(IUnitOfWork unitOfWork, DataSourceProvider dataSources, DataFormatMan formatMan)
        {
            this.unitOfWork = unitOfWork;
            this.dataSources = dataSources;
            this.formatMan = formatMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public AssetBase GetAsset(string id)
        {
            AssetBase asset = null;
            if (_openedAssets.TryGetValue(id, out asset))
                return asset;

            var entry = unitOfWork.GetRepository<IAssetEntry>().GetById(id);
            if (entry == null)
                throw new Exception($"Asset error: {id}");

            asset = CreateAsset(entry);

            return asset;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void LockSource(AssetBase source)
        {
            _openedAssets.Add(source.Id, source);
        }

        internal void ReleaseSource(AssetBase source)
        {
            _openedAssets.Remove(source.Id);
        }

        #endregion Internal Methods

        #region Private Methods

        private AssetBase CreateAsset(IAssetEntry assetEntry)
        {
            var formatType = formatMan.GetFormatType(assetEntry.FormatType);

            if (formatType == null)
                throw new Exception($"Unknown format {assetEntry.FormatType}");

            var ds = dataSources.GetDataSource(assetEntry.DataSourceRef);

            if (ds == null)
                throw new Exception($"Unknown DataSourceRef {assetEntry.DataSourceRef}");

            return new AssetBase(this, assetEntry.Id, ds, formatType, assetEntry.Parameters);
        }

        #endregion Private Methods
    }
}