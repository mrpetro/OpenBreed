using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Sources;
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

        private readonly Dictionary<string, IDataFormat> _formats = new Dictionary<string, IDataFormat>();

        #endregion Private Fields

        #region Public Methods

        public object Load(BaseSource source, FormatDef formatDef)
        {
            var format = GetFormat(formatDef.Name);
            if (format == null)
                throw new Exception($"Unknown format {formatDef.Name}");

            var parameters = GetParameters(formatDef.Parameters);

            return source.Load(format, parameters);
        }

        public void RegisterFormat(string formatAlias, IDataFormat format)
        {
            if (_formats.ContainsKey(formatAlias))
                throw new InvalidOperationException($"Format alias '{formatAlias}' already registered.");

            _formats.Add(formatAlias, format);
        }

        #endregion Public Methods

        #region Private Methods

        private IDataFormat GetFormat(string formatType)
        {
            IDataFormat sourceMan = null;
            if (_formats.TryGetValue(formatType, out sourceMan))
                return sourceMan;
            else
                throw new InvalidOperationException("Unknown format: " + formatType);
        }

        private Dictionary<string, object> GetParameters(List<FormatParameterDef> parameterDefs)
        {
            var parameters = new Dictionary<string, object>();

            foreach (var parameterDef in parameterDefs)
            {
                if (string.IsNullOrWhiteSpace(parameterDef.Name) ||
                    string.IsNullOrWhiteSpace(parameterDef.Type))
                    continue;

                if (parameters.ContainsKey(parameterDef.Name))
                    continue;

                var type = Type.GetType(parameterDef.Type);
                var tc = TypeDescriptor.GetConverter(type);
                var value = tc.ConvertFromString(null, CultureInfo.InvariantCulture, parameterDef.Value);
                parameters.Add(parameterDef.Name, value);
            }

            return parameters;
        }

        #endregion Private Methods
    }
}
