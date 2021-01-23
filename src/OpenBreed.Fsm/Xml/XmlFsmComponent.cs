using OpenBreed.Components.Common.Xml;
using OpenBreed.Ecsw.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Fsm.Xml
{
    public class XmlMachineStateTemplate : IMachineStateTemplate
    {
        #region Public Properties

        [XmlAttribute("FsmName")]
        public string FsmName { get; set; }

        [XmlAttribute("StateName")]
        public string StateName { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("Fsm")]
    public class XmlFsmComponent : XmlComponentTemplate, IFsmComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IMachineStateTemplate> States => XmlStates.Cast<IMachineStateTemplate>();

        [XmlArray("States")]
        [XmlArrayItem(ElementName = "State")]
        public XmlMachineStateTemplate[] XmlStates { get; set; }

        #endregion Public Properties
    }
}