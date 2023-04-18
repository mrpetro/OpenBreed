using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Sandbox.Wecs.Components.Xml
{
    [XmlRoot("Lives")]
    public class XmlLivesComponent : XmlComponentTemplate, ILivesComponentTemplate
    {
        [XmlElement("Value")]
        public int Value { get; set; }
    }
}
