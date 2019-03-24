using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Palettes
{
    public interface IPaletteFromMapEntry : IPaletteEntry
    {
        #region Public Properties

        string BlockName { get; set; }
        string DataRef { get; set; }

        #endregion Public Properties
    }
}
