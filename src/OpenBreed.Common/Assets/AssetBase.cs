using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;

namespace OpenBreed.Common.Assets
{
    public class AssetBase
    {
        #region Protected Fields

        protected readonly AssetsDataProvider provider;

        #endregion Protected Fields

        #region Private Fields

        private readonly DataSourceBase dataSource;
        private readonly List<FormatParameter> parameters;
        private IDataFormatType formatType;

        #endregion Private Fields

        #region Internal Constructors

        internal AssetBase(AssetsDataProvider provider, string id, DataSourceBase dataSource, IDataFormatType formatType, List<FormatParameter> parameters)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));

            Id = id;
            this.dataSource = dataSource;
            this.formatType = formatType;
            this.parameters = parameters;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public object Load()
        {
            return formatType.Load(dataSource, parameters);
        }

        public void Save(object model)
        {
            formatType.Save(dataSource, model, parameters);
        }

        #endregion Public Methods
    }
}