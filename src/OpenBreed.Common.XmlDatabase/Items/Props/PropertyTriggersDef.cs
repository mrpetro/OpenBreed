using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Props
{
    public class PropertyTriggersDef : IPropertyTriggers
    {
        [XmlElement("OnLoad")]
        public string OnLoad { get; set; }
        [XmlElement("OnDestroy")]
        public string OnDestroy { get; set; }
        [XmlElement("OnCollisionEnter")]
        public string OnCollisionEnter { get; set; }
        [XmlElement("OnCollisionLeave")]
        public string OnCollisionLeave { get; set; }
    }
}
