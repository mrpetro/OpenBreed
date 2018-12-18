using OpenBreed.Common.Database.Items.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Tables.Palettes
{
    public class DatabasePaletteTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Palette", typeof(PaletteDef))]
        public readonly List<PaletteDef> Items = new List<PaletteDef>();
    }
}
