using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase.Items.Props
{
    [Serializable]
    public class PropertyDef : IPropertyEntry
    {

        #region Public Properties

        public string Description { get; set; }

        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public IPropertyPresentation Presentation { get; set; }

        [XmlIgnore]
        public IPropertyTriggers Triggers { get; set; }

        [XmlElement("Presentation")]
        public PropertyPresentationDef XmlPresentation
        {
            get
            {
                return (PropertyPresentationDef)Presentation;
            }

            set
            {
                Presentation = value;
            }
        }

        [XmlElement("Triggers")]
        public PropertyTriggersDef XmlTriggers
        {
            get
            {
                return (PropertyTriggersDef)Triggers;
            }

            set
            {
                Triggers = value;
            }
        }

        #endregion Public Properties

    }
}
