using OpenBreed.Common.XmlDatabase.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Sprites
{
    public class XmlDbSpriteSetTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("SpriteSetFromSPR", typeof(XmlSpriteSetFromSprEntry)),
        XmlArrayItem("SpriteSetFromImage", typeof(XmlSpriteSetFromImageEntry))]
        public readonly List<XmlSpriteSetEntry> Items = new List<XmlSpriteSetEntry>();
    }
}
