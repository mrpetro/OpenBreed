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
using OpenBreed.Database.Xml.Items.Palettes;

namespace OpenBreed.Database.Xml.Items.Maps
{
    [Serializable]
    [Description("Map"), Category("Appearance")]
    public class XmlDbMap : XmlDbEntry, IDbMap
    {
        #region Public Constructors

        public XmlDbMap()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbMap(XmlDbMap other) : base(other)
        {
            DataRef = other.DataRef;
            Format = other.Format;
            PaletteRefs = other.PaletteRefs.ToList();
            ActionSetRef = other.ActionSetRef;
            SpriteSetRefs = other.SpriteSetRefs.ToList();
            TileSetRef = other.TileSetRef;
            ScriptRef = other.ScriptRef;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("DataRef")]
        public string DataRef { get; set; }

        [XmlElement("Format")]
        public string Format { get; set; }

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; init; } = new List<string>();

        [XmlElement("ActionSetRef")]
        public string ActionSetRef { get; set; }

        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; init; } = new List<string>();

        [XmlElement("TileSetRef")]
        public string TileSetRef { get; set; }

        [XmlElement("ScriptRef")]
        public string ScriptRef { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy() => new XmlDbMap(this);

        #endregion Public Methods
    }
}