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
    [Description("Palette from binary"), Category("Appearance")]
    public class XmlDbPaletteFromBinary : XmlDbPalette, IDbPaletteFromBinary
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("ColorsNo")]
        public int ColorsNo { get; set; }

        [XmlElement("DataStart")]
        public int DataStart { get; set; }

        [XmlElement("Mode")]
        public PaletteMode Mode { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new XmlDbPaletteFromBinary()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }

        #endregion Public Methods
    }
}
