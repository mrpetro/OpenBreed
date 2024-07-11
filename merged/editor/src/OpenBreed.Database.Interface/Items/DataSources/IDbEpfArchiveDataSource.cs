using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.DataSources
{
    public interface IDbEpfArchiveDataSource : IDbDataSource
    {
        string ArchivePath { get; set; }
        string EntryName { get; set; }
    }
}
