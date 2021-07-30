using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    [Serializable]
    public class XmlDbAction : IDbAction
    {
        #region Public Properties

        public string Description { get; set; }

        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public IDbActionPresentation Presentation { get; private set; } = new XmlDbActionPresentation();

        [XmlIgnore]
        public IDbActionTriggers Triggers { get; private set; } = new XmlDbActionTriggers();

        [XmlElement("Presentation")]
        public XmlDbActionPresentation XmlPresentation
        {
            get
            {
                return (XmlDbActionPresentation)Presentation;
            }

            set
            {
                Presentation = value;
            }
        }

        [XmlElement("Triggers")]
        public XmlDbActionTriggers XmlTriggers
        {
            get
            {
                return (XmlDbActionTriggers)Triggers;
            }

            set
            {
                Triggers = value;
            }
        }

        #endregion Public Properties

    }
}
