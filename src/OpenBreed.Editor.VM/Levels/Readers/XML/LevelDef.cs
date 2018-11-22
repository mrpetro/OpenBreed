using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Editor.VM.Levels.Readers.XML
{
    [Serializable]
    public class LevelDef
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public string MapResourceRef { get; set; }
        public string TileSetResourceRef { get; set; }
        public string PropertySetResourceRef { get; set; }
        [XmlArray("SpriteSetResourceRefs"),
        XmlArrayItem("SpriteSetResourceRef", typeof(string))]
        public List<string> SpriteSetResourceRefs { get; set; }

        public override string ToString()
        {
            return Id.ToString() + ". " + Name;
        }
    }
}
