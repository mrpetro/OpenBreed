using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.DataSources
{
    public interface IEPFArchiveDataSourceEntry : IDataSourceEntry
    {
        string ArchivePath { get; set; }
        string EntryName { get; set; }
    }
}
