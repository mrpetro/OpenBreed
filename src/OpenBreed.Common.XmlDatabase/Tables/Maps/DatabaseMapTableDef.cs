using OpenBreed.Common.XmlDatabase.Items.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Maps
{
    public class DatabaseMapTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Map", typeof(XmlMapEntry))]
        public readonly List<XmlMapEntry> Items = new List<XmlMapEntry>();
    }
}
