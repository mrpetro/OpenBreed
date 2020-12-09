using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Core.Components.Xml
{
    [XmlRoot("AngularPosition")]
    public class XmlAngularPositionComponent : XmlComponentTemplate, IAngularPositionComponentTemplate
    {
        [XmlElement("Value")]
        public float Value { get; set; }
    }
}
