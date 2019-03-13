using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Palettes
{
    public enum PaletteMode
    {
        COLOR_16BIT,
        COLOR_32BIT
    }

    public interface IPaletteFromBinaryEntry : IPaletteEntry
    {
        PaletteMode Mode { get; set; }
        int ColorsNo { get; set; }
        int DataStart { get; set; }
    }
}
