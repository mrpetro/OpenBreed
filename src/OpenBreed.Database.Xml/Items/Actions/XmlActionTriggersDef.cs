using OpenBreed.Database.Interface.Items.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Actions
{
    public class XmlActionTriggersDef : IActionTriggers
    {
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
    }
}
