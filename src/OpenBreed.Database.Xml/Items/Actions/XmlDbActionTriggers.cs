using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    public class XmlDbActionTriggers : IDbActionTriggers
    {
        #region Public Constructors

        public XmlDbActionTriggers()
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlDbActionTriggers(XmlDbActionTriggers other)
        {
            OnCollisionEnter = other.OnCollisionEnter;
            OnCollisionLeave = other.OnCollisionLeave;
            OnDestroy = other.OnDestroy;
            OnLoad = other.OnLoad;
        }

        #endregion Protected Constructors

        #region Public Properties

        [XmlElement("OnCollisionEnter")]
        public string OnCollisionEnter { get; set; }

        [XmlElement("OnCollisionLeave")]
        public string OnCollisionLeave { get; set; }

        [XmlElement("OnDestroy")]
        public string OnDestroy { get; set; }

        [XmlElement("OnLoad")]
        public string OnLoad { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDbActionTriggers Copy() => new XmlDbActionTriggers(this);

        #endregion Public Methods
    }
}