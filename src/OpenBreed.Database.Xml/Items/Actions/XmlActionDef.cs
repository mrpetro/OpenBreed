using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    public class XmlActionDef : IActionEntry
    {
        #region Public Properties

        public string Description { get; set; }

        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public IActionPresentation Presentation { get; private set; } = new XmlActionPresentationDef();

        [XmlIgnore]
        public IActionTriggers Triggers { get; private set; } = new XmlActionTriggersDef();

        [XmlElement("Presentation")]
        public XmlActionPresentationDef XmlPresentation
        {
            get
            {
                return (XmlActionPresentationDef)Presentation;
            }

            set
            {
                Presentation = value;
            }
        }

        [XmlElement("Triggers")]
        public XmlActionTriggersDef XmlTriggers
        {
            get
            {
                return (XmlActionTriggersDef)Triggers;
            }

            set
            {
                Triggers = value;
            }
        }

        #endregion Public Properties

    }
}
