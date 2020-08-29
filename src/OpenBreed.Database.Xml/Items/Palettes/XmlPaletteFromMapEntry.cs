using OpenBreed.Common;
using OpenBreed.Common.Palettes;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Palettes
{
    [Serializable]
    [Description("Palette from MAP"), Category("Appearance")]
    public class XmlPaletteFromMapEntry : XmlPaletteEntry, IPaletteFromMapEntry
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        public override IEntry Copy()
        {
            return new XmlPaletteFromMapEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef,
                BlockName = this.BlockName
            };
        }
    }
}
