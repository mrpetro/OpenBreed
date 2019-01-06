using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class DataFormat
    {
        private IDataFormatType _type;
        private AssetBase _asset;
        private List<FormatParameter> _parameters;
        public DataFormat(IDataFormatType type, AssetBase asset, List<FormatParameter> parameters)
        {
            _type = type;
            _asset = asset;
            _parameters = parameters;
        }

        public object Load()
        {
            return _asset.Load(_type, _parameters);
        }
    }
}
