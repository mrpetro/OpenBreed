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
    [Description("Palette from MAP"), Category("Appearance")]
    public class XmlDbPaletteFromMap : XmlDbPalette, IDbPaletteFromMap
    {
        #region Public Constructors

        public XmlDbPaletteFromMap()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbPaletteFromMap(XmlDbPaletteFromMap other) : base(other)
        {
            MapRef = other.MapRef;
            BlockName = other.BlockName;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("MapRef")]
        public string MapRef { get; set; }

        [XmlElement("BlockName")]
        public string BlockName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbPaletteFromMap(this);

        #endregion Public Methods
    }
}