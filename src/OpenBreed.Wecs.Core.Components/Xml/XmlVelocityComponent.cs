using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("Velocity")]
    public class XmlVelocityComponent : XmlComponentTemplate, IVelocityComponentTemplate
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
            return VelocityComponent.Create(X, Y);
        }

        #endregion Public Methods
    }
}