﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Palettes
{
    public enum PaletteMode
    {
        COLOR_16BIT,
        COLOR_32BIT,
        VGA_24BIT
    }

    /// <summary>
    /// Palette entry interface where source palette comes from binary data
    /// </summary>
    public interface IDbPaletteFromBinary : IDbPalette
    {
        #region Public Properties

        int ColorsNo { get; set; }
        string DataRef { get; set; }
        int DataStart { get; set; }
        PaletteMode Mode { get; set; }

        #endregion Public Properties
    }
}
