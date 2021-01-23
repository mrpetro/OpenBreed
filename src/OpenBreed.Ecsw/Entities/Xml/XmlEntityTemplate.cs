using OpenBreed.Ecsw.Components;
using OpenBreed.Ecsw.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Ecsw.Entities.Xml
{
    [Serializable]
    [XmlRoot("EntityTemplate")]
    public class XmlEntityTemplate : IEntityTemplate
    {
        #region Public Properties

        [XmlElement("Components")]
        public XmlComponentsList XmlComponents { get; set; }

        [XmlIgnore]
        public IEnumerable<IComponentTemplate> Components => XmlComponents.Cast<IComponentTemplate>();

        #endregion Public Properties
    }
}