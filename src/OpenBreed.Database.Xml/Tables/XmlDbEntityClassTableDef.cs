using OpenBreed.Database.Xml.Items.EntityClass;
using OpenBreed.Database.Xml.Items.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Tables
{
    public class XmlDbEntityClassTableDef : XmlDbTableDef
    {
        public const string NAME = "EntityClasses";

        [XmlIgnore]
        public override string Name => NAME;

        [XmlArray("Items"),
        XmlArrayItem("EntityClass", typeof(XmlDbEntityClass))]
        public readonly List<XmlDbEntityClass> Items = new List<XmlDbEntityClass>();
    }
}
