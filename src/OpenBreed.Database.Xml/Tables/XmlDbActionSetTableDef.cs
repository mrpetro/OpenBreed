using OpenBreed.Database.Xml.Items.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbActionSetTableDef : XmlDbTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("ActionSet", typeof(XmlActionSetEntry))]
        public readonly List<XmlActionSetEntry> Items = new List<XmlActionSetEntry>();
    }
}
