using OpenBreed.Common.Formats;
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
    [Description("Palette"), Category("Appearance")]
    public class XmlPaletteEntry : XmlDbEntry, IPaletteEntry
    {

        #region Public Properties

        [XmlIgnore]
        public IPaletteData Data { get; set; }

        [XmlElement("FromMAP", Type = typeof(XmlPaletteDataFromMap)),
        XmlElement("FromBinary", Type = typeof(XmlPaletteDataFromBinary))]
        public XmlPaletteData XmlData
        {
            get
            {
                return (XmlPaletteData)Data;
            }

            set
            {
                Data = value;
            }
        }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}
