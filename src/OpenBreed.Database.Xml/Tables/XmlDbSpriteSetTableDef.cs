using OpenBreed.Database.Xml.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbSpriteSetTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("SpriteSetFromSPR", typeof(XmlSpriteSetFromSprEntry)),
        XmlArrayItem("SpriteSetFromImage", typeof(XmlSpriteSetFromImageEntry))]
        public readonly List<XmlSpriteSetEntry> Items = new List<XmlSpriteSetEntry>();
    }
}
