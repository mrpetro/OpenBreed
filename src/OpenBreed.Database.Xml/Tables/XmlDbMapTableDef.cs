using OpenBreed.Database.Xml.Items.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbMapTableDef : XmlDbTableDef
    {
        public const string NAME = "Maps";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("Map", typeof(XmlMapEntry))]
        public readonly List<XmlMapEntry> Items = new List<XmlMapEntry>();
    }
}
