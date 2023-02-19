﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Palettes
{
    /// <summary>
    /// Palette entry interface where source palette comes from LMB image
    /// </summary>
    public interface IDbPaletteFromLbm : IDbPalette
    {
        #region Public Properties

        string DataRef { get; set; }

        #endregion Public Properties
    }
}
