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
        #region Public Constructors

        public XmlDbPaletteFromLbm()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbPaletteFromLbm(XmlDbPaletteFromLbm other) : base(other)
        {
            DataRef = other.DataRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbPaletteFromLbm(this);

        #endregion Public Methods
    }
}