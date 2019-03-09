using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.XmlDatabase.Items.Palettes
{
    [Serializable]
    [Description("Palette from binary"), Category("Appearance")]
    public class XmlPaletteFromBinaryEntry : XmlPaletteEntry, IPaletteFromBinaryEntry
    {
        public override IEntry Copy()
        {
            return new XmlPaletteFromBinaryEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}
