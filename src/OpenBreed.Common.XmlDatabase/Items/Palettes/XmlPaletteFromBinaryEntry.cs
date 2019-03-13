using OpenBreed.Common.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Palettes
{
    [Serializable]
    [Description("Palette from binary"), Category("Appearance")]
    public class XmlPaletteFromBinaryEntry : XmlPaletteEntry, IPaletteFromBinaryEntry
    {
        #region Public Properties

        [XmlElement("ColorsNo")]
        public int ColorsNo { get; set; }

        [XmlElement("DataStart")]
        public int DataStart { get; set; }

        [XmlElement("Mode")]
        public PaletteMode Mode { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntry Copy()
        {
            return new XmlPaletteFromBinaryEntry()
            {
                Id = this.Id,
                Description = this.Description,
                DataRef = this.DataRef
            };
        }

        #endregion Public Methods
    }
}
