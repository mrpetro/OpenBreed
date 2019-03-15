using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Assets
{
    public interface IAssetEntry : IEntry
    {
        IFormatEntry Format { get; }

    }
}
