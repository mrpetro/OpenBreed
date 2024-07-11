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
        #region Public Constructors

        public XmlDbAction()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbAction(XmlDbAction other)
        {
            Id = other.Id;
            Description = other.Description;
            Name = other.Name;
            XmlPresentation = (XmlDbActionPresentation)other.XmlPresentation?.Copy();
            XmlTriggers = (XmlDbActionTriggers)other.XmlTriggers?.Copy();
        }

        #endregion Protected Constructors

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

        #region Public Methods

        public IDbAction Copy() => new XmlDbAction(this);

        #endregion Public Methods
    }
}