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

        public DataFormat Create(AssetBase asset, string formatType, List<FormatParameter> formatParameters)
        {
            var ft = GetFormatType(formatType);
            if (ft == null)
                throw new InvalidOperationException("Unknown format type: " + formatType);

            return new DataFormat(ft, asset, formatParameters);
        }

        public void RegisterFormat(string formatType, IDataFormatType format)
        {
            if (_formats.ContainsKey(formatType))
                throw new InvalidOperationException($"Format type '{formatType}' already registered.");

            _formats.Add(formatType, format);
        }

        #endregion Public Methods

        #region Internal Methods

        internal IDataFormatType GetFormatType(string formatType)
        {
            IDataFormatType ft = null;
            if (_formats.TryGetValue(formatType, out ft))
                return ft;
            else
                throw new InvalidOperationException("Unknown format type: " + formatType);
        }

        #endregion Internal Methods

    }
}
