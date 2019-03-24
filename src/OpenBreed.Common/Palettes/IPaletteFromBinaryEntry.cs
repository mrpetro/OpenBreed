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
        #region Public Properties

        int ColorsNo { get; set; }
        string DataRef { get; set; }
        int DataStart { get; set; }
        PaletteMode Mode { get; set; }

        #endregion Public Properties
    }
}
