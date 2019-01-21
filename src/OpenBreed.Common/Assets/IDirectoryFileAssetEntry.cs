using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Assets
{
    public interface IDirectoryFileAssetEntry : IAssetEntry
    {
        string DirectoryPath { get; }
        string FileName { get; set; }
    }
}
