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
    [XmlRoot("Ammo")]
    public class XmlAmmoComponent : XmlComponentTemplate, IHealthComponentTemplate
    {
        [XmlElement("MaximumValue")]
        public int MaximumValue { get; set; }

        [XmlElement("Value")]
        public int Value { get; set; }
    }
}
