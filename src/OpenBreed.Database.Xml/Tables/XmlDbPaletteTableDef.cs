using OpenBreed.Database.Xml.Items.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbPaletteTableDef : XmlDbTableDef
    {
        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("PaletteFromBinary", typeof(XmlPaletteFromBinaryEntry)),
        XmlArrayItem("PaletteFromMAP", typeof(XmlPaletteFromMapEntry))]
        public readonly List<XmlPaletteEntry> Items = new List<XmlPaletteEntry>();

        #endregion Public Fields
    }
}
