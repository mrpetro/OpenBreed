using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.DataSources
{
    public interface IEPFArchiveDataSourceEntry : IDataSourceEntry
    {
        string ArchivePath { get; set; }
        string EntryName { get; set; }
    }
}
