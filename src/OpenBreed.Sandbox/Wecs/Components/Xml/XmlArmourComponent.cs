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
    [XmlRoot("Armour")]
    public class XmlArmourComponent : XmlComponentTemplate, IArmourComponentTemplate
    {
        [XmlElement("Value")]
        public int Value { get; set; }
    }
}
