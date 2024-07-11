using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Xml.Items.Images;
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
        #region Public Constructors

        public XmlDbPaletteFromBinary()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbPaletteFromBinary(XmlDbPaletteFromBinary other) : base(other)
        {
            DataRef = other.DataRef;
            ColorsNo = other.ColorsNo;
            DataStart = other.DataStart;
            Mode = other.Mode;
        }

        #endregion Protected Constructors

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

        public override IDbEntry Copy() => new XmlDbPaletteFromBinary(this);

        #endregion Public Methods
    }
}