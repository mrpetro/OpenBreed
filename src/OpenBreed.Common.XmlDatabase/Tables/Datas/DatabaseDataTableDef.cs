using OpenBreed.Common.XmlDatabase.Items.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Tables.Datas
{
    public class DatabaseDataTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("Data", typeof(XmlDataEntry))]
        public readonly List<XmlDataEntry> Items = new List<XmlDataEntry>();
    }
}
