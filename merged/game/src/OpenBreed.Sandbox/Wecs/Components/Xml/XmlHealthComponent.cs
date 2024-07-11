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
    [XmlRoot("Health")]
    public class XmlHealthComponent : XmlComponentTemplate, IHealthComponentTemplate
    {
        [XmlElement("MaximumValue")]
        public int MaximumRoundsCount { get; set; }

        [XmlElement("Value")]
        public int RoundsCount { get; set; }
    }
}
