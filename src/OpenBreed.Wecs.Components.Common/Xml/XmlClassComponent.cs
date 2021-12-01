using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common.Xml
{
    [XmlRoot("Class")]
    public class XmlClassComponent : XmlComponentTemplate, IClassComponentTemplate
    {
        [XmlElement("Level")]
        public string Level { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Flavor")]
        public string Flavor { get; set; }
    }
}
