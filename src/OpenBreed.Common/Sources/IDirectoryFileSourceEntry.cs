using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Sources
{
    public interface IDirectoryFileSourceEntry : ISourceEntry
    {
        string DirectoryPath { get; }
    }
}
