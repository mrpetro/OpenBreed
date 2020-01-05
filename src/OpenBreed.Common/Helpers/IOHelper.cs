using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Helpers
{
    /// <summary>
    /// Helper class with various file system related methods
    /// </summary>
    public static class IOHelper
    {
        public static string GetNormalizedPath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}
