using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Palettes
{
    /// <summary>
    /// Palette entry interface where source palette comes from MapModel palette blocks
    /// </summary>
    public interface IDbPaletteFromMap : IDbPalette
    {
        #region Public Properties

        string BlockName { get; set; }
        string MapRef { get; set; }

        #endregion Public Properties
    }
}
