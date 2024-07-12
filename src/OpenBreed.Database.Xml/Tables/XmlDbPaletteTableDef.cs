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
        public const string NAME = "Palettes";

        [XmlIgnore]
        public override string Name => NAME;

        #region Public Fields

        [XmlArray("Items"),
        XmlArrayItem("PaletteFromBinary", typeof(XmlDbPaletteFromBinary)),
        XmlArrayItem("PaletteFromMAP", typeof(XmlDbPaletteFromMap)),
        XmlArrayItem("PaletteFromLBM", typeof(XmlDbPaletteFromLbm))]
        public readonly List<XmlDbPalette> Items = new List<XmlDbPalette>();

        #endregion Public Fields
    }
}
