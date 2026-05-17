using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("Thrust")]
    public class XmlThrustComponent : XmlComponentTemplate, IThrustComponentTemplate
    {
        #region Public Properties

        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return ThrustComponent.Create(X, Y);
        }

        #endregion Public Methods
    }
}