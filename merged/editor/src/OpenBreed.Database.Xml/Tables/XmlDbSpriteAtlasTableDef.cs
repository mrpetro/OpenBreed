using OpenBreed.Database.Xml.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbSpriteAtlasTableDef : XmlDbTableDef
    {
        public const string NAME = "SpriteAtlases";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("SpriteSetFromSPR", typeof(XmlDbSpriteAtlasFromSpr)),
        XmlArrayItem("SpriteSetFromImage", typeof(XmlDbSpriteAtlasFromImage))]
        public readonly List<XmlDbSpriteAtlas> Items = new List<XmlDbSpriteAtlas>();
    }
}
