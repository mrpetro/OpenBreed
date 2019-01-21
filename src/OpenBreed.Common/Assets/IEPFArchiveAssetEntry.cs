using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Assets
{
    public interface IEPFArchiveAssetEntry : IAssetEntry
    {
        string ArchivePath { get; }
        string EntryName { get; set; }
    }
}
