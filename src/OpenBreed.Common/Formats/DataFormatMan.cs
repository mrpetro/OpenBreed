using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class DataFormatMan
    {
        #region Private Fields

        private readonly Dictionary<string, IDataFormatType> _formats = new Dictionary<string, IDataFormatType>();

        #endregion Private Fields

        #region Public Methods


        public DataFormat Create(AssetBase asset, IFormatEntry format)
        {
            var formatType = GetFormatType(format.Name);
            if (formatType == null)
                throw new Exception($"Unknown format {format.Name}");

            return new DataFormat(formatType, asset, format.Parameters);
        }

        public object Load(AssetBase asset, IFormatEntry format)
        {
            var formatType = GetFormatType(format.Name);
            if (formatType == null)
                throw new Exception($"Unknown format {format.Name}");

            return asset.Load(formatType, format.Parameters);
        }

        public void Save(AssetBase asset, object data, IFormatEntry format)
        {
            var formatType = GetFormatType(format.Name);
            if (formatType == null)
                throw new Exception($"Unknown format {format.Name}");

            asset.Save(data, formatType, format.Parameters);
        }

        public void RegisterFormat(string formatAlias, IDataFormatType format)
        {
            if (_formats.ContainsKey(formatAlias))
                throw new InvalidOperationException($"Format alias '{formatAlias}' already registered.");

            _formats.Add(formatAlias, format);
        }

        #endregion Public Methods

        #region Private Methods

        private IDataFormatType GetFormatType(string formatType)
        {
            IDataFormatType sourceMan = null;
            if (_formats.TryGetValue(formatType, out sourceMan))
                return sourceMan;
            else
                throw new InvalidOperationException("Unknown format: " + formatType);
        }

        #endregion Private Methods
    }
}
