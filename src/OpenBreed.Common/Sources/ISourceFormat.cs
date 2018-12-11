using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Sources
{
    public interface ISourceFormat
    {
        object Load(BaseSource sourceModel);
        void Save(BaseSource sourceModel, object model);
    }
}
