using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public interface IDataFormatType
    {
        object Load(AssetBase asset, List<FormatParameter> parameters);
        void Save(AssetBase asset, object model, List<FormatParameter> parameters);
    }
}
