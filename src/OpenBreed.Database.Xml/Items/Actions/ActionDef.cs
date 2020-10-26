using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    public class ActionDef : IActionEntry
    {
        #region Public Properties

        public string Description { get; set; }

        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public IActionPresentation Presentation { get; private set; } = new ActionPresentationDef();

        [XmlIgnore]
        public IActionTriggers Triggers { get; private set; } = new ActionTriggersDef();

        [XmlElement("Presentation")]
        public ActionPresentationDef XmlPresentation
        {
            get
            {
                return (ActionPresentationDef)Presentation;
            }

            set
            {
                Presentation = value;
            }
        }

        [XmlElement("Triggers")]
        public ActionTriggersDef XmlTriggers
        {
            get
            {
                return (ActionTriggersDef)Triggers;
            }

            set
            {
                Triggers = value;
            }
        }

        #endregion Public Properties

    }
}
