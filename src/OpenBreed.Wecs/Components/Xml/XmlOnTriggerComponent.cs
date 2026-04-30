using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Xml
{
    public class XmlOnTriggerActionTemplate : IOnTriggerActionTemplate
    {
        #region Public Properties

        [XmlAttribute("Trigger")]
        public string Trigger { get; set; }

        [XmlAttribute("Action")]
        public string Action { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("OnTrigger")]
    public class XmlOnTriggerComponent : XmlComponentTemplate, IOnTriggerComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IOnTriggerActionTemplate> Actions => XmlActions.Cast<IOnTriggerActionTemplate>();

        [XmlArray("Actions")]
        [XmlArrayItem(ElementName = "Action")]
        public XmlOnTriggerActionTemplate[] XmlActions { get; set; } = Array.Empty<XmlOnTriggerActionTemplate>();

        #endregion Public Properties
    }
}