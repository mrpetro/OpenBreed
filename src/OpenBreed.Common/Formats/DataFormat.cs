using OpenBreed.Common.Assets;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class DataFormat
    {
        #region Private Fields

        private readonly List<FormatParameter> _parameters;
        private readonly IDataFormatType _type;
        private AssetBase _asset;

        #endregion Private Fields

        #region Public Constructors

        public DataFormat(IDataFormatType type, AssetBase asset, List<FormatParameter> parameters)
        {
            _type = type;
            _asset = asset;
            _parameters = parameters;
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load()
        {
            return _asset.Load();
        }

        #endregion Public Methods
    }
}
