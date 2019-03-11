using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Palettes
{
    public interface IPaletteFromMapEntry : IPaletteEntry
    {
        string BlockName { get; set; }
    }
}
