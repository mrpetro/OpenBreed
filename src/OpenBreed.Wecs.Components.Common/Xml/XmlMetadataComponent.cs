using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common.Xml
{
    [XmlRoot("Metadata")]
    public class XmlMetadataComponent : XmlComponentTemplate, IMetadataComponentTemplate
    {
        [XmlElement("Level")]
        public string Level { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Flavor")]
        public string Flavor { get; set; }
    }
}
