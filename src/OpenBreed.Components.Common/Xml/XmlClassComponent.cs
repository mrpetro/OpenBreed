using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Components.Common.Xml
{
    [XmlRoot("Class")]
    public class XmlClassComponent : XmlComponentTemplate, IClassComponentTemplate
    {
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
