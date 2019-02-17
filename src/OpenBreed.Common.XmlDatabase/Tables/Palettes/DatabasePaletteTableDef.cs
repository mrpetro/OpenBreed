using OpenBreed.Common.XmlDatabase.Items.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Palettes
{
    public class DatabasePaletteTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Palette", typeof(XmlPaletteEntry))]
        public readonly List<XmlPaletteEntry> Items = new List<XmlPaletteEntry>();
    }
}
