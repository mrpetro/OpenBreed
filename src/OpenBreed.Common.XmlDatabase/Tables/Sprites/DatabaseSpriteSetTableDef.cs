using OpenBreed.Common.XmlDatabase.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Sprites
{
    public class DatabaseSpriteSetTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("SpriteSet", typeof(XmlSpriteSetEntry))]
        public readonly List<XmlSpriteSetEntry> Items = new List<XmlSpriteSetEntry>();
    }
}
