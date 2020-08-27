using OpenBreed.Common.Assets;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public interface IDataFormatType
    {
        object Load(DataSourceBase ds, List<FormatParameter> parameters);
        void Save(DataSourceBase ds, object model, List<FormatParameter> parameters);
    }
}
