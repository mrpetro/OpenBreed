using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public interface IDataFormat
    {
        object Load(BaseSource sourceModel);
        void Save(BaseSource sourceModel, object model);
    }
}
