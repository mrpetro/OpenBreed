using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Resurrectable")]
    public class XmlResurrectableComponent : XmlComponentTemplate, IResurrectableComponentTemplate
    {
        [XmlElement("WorldName")]
        public string WorldName { get; set; }
    }
}
