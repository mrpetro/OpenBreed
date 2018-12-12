using OpenBreed.Common.Database.Items.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Database.Tables.Props
{
    public class DatabasePropertySetTableDef : DatabaseTableDef
    {
        [XmlArray("Items"),
        XmlArrayItem("PropertySet", typeof(PropertySetDef))]
        public readonly List<PropertySetDef> Items = new List<PropertySetDef>();
    }
}
