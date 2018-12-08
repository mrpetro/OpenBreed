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
        [XmlAttribute]
        public int Id { get; set; }

        public string MapResourceRef { get; set; }
        public string TileSetResourceRef { get; set; }
        public string PropertySetResourceRef { get; set; }
        [XmlArray("SpriteSetResourceRefs"),
        XmlArrayItem("SpriteSetResourceRef", typeof(string))]
        public List<string> SpriteSetResourceRefs { get; set; }
    }
}
