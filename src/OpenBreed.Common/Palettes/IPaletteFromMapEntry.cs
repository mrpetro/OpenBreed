using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Palettes
{
    /// <summary>
    /// Palette entry interface where source palette comes from MapModel palette blocks
    /// </summary>
    public interface IPaletteFromMapEntry : IPaletteEntry
    {
        #region Public Properties

        string BlockName { get; set; }
        string DataRef { get; set; }

        #endregion Public Properties
    }
}
