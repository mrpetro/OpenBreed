using OpenBreed.Common;
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
    [Description("Palette from LBM"), Category("Appearance")]
    public class XmlDbPaletteFromLbm : XmlDbPalette, IDbPaletteFromLbm
    {
        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        public override IDbEntry Copy()
        {
            return new XmlDbPaletteFromLbm()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }
    }
}
