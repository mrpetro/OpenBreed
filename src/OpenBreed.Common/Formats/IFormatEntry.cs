using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public interface IFormatEntry
    {
        string Name { get; }
        List<FormatParameter> Parameters { get; }
    }
}
