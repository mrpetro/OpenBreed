using OpenBreed.Common.Sources;
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
        private SourceBase _dataSource;
        private List<FormatParameter> _parameters;
        public DataFormat(IDataFormatType type, SourceBase dataSource, List<FormatParameter> parameters)
        {
            _type = type;
            _dataSource = dataSource;
            _parameters = parameters;
        }

        public object Load()
        {
            return _dataSource.Load(_type, _parameters);
        }
    }
}
