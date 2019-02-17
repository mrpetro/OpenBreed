using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace OpenBreed.Common.XmlDatabase.Items.Maps
{
    [Serializable]
    [Description("Map"), Category("Appearance")]
    public class XmlMapEntry : XmlDbEntry, IMapEntry
    {
        #region Public Properties

        [XmlIgnore]
        public IFormatEntry Format { get; set; }

        [XmlElement("Format")]
        public XmlFormatEntry FormatDef
        {
            get
            {
                return (XmlFormatEntry)Format;
            }

            set
            {
                Format = value;
            }
        }

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        [XmlElement("ActionSetRef")]
        public string ActionSetRef { get; set; }

        public string AssetRef { get; set; }

        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; } = new List<string>();

        public string TileSetRef { get; set; }

        public override IEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

    }
}
