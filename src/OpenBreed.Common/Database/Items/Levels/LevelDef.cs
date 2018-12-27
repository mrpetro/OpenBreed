using OpenBreed.Common.Database.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Items.Levels
{
    [Serializable]
    public class LevelDef : DatabaseItemDef
    {
        public string SourceRef { get; set; }
        public FormatDef Format { get; set; }

        public string TileSetRef { get; set; }
        public string PropertySetRef { get; set; }

        [XmlArray("SpriteSetRefs"),
        XmlArrayItem("SpriteSetRef", typeof(string))]
        public List<string> SpriteSetRefs { get; } = new List<string>();

        [XmlArray("PaletteRefs"),
        XmlArrayItem("PaletteRef", typeof(string))]
        public List<string> PaletteRefs { get; } = new List<string>();

    }
}
