using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class DataFormatMan
    {
        private readonly Dictionary<string, IDataFormat> _formats = new Dictionary<string, IDataFormat>();

        public void RegisterFormat(string formatAlias, IDataFormat format)
        {
            if (_formats.ContainsKey(formatAlias))
                throw new InvalidOperationException($"Format alias '{formatAlias}' already registered.");

            _formats.Add(formatAlias, format);
        }

        public IDataFormat GetFormatMan(string formatType)
        {
            IDataFormat sourceMan = null;
            if (_formats.TryGetValue(formatType, out sourceMan))
                return sourceMan;
            else
                throw new InvalidOperationException("Unknown format: " + formatType);
        }
    }
}
