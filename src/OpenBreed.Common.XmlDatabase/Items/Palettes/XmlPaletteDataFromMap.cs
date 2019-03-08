using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Palettes
{
    public class XmlPaletteDataFromMap : XmlPaletteData, IPaletteDataFromMap
    {
        [XmlElement("BlockName")]
        public string BlockName { get; set; }
    }
}
