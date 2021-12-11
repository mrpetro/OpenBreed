using OpenBreed.Database.Xml.Items.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;

namespace OpenBreed.Database.Xml.Items.Maps
{
    [Serializable]
    [Description("Map"), Category("Appearance")]
    public class XmlDbMap : XmlDbEntry, IDbMap
    {
        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

        [XmlElement("ActionSetRef")]
        public string ActionSetRef { get; set; }

        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; } = new List<string>();

        [XmlElement("TileSetRef")]
        public string TileSetRef { get; set; }

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

    }
}
