using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public interface IDataFormatType
    {
        object Load(SourceBase sourceModel, Dictionary<string, object> parameters);
        void Save(SourceBase sourceModel, object model);
    }
}
