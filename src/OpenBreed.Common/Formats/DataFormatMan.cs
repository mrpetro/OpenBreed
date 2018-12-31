using OpenBreed.Common.Sources;
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


        public DataFormat Create(SourceBase source, IFormatEntity format)
        {
            var formatType = GetFormatType(format.Name);
            if (formatType == null)
                throw new Exception($"Unknown format {format.Name}");

            var parameters = format.Parameters;

            return new DataFormat(formatType, source, parameters);
        }

        public object Load(SourceBase source, IFormatEntity format)
        {
            var formatType = GetFormatType(format.Name);
            if (formatType == null)
                throw new Exception($"Unknown format {format.Name}");

            var parameters = format.Parameters;

            return source.Load(formatType, parameters);
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
