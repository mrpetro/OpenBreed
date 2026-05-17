using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("PreviousPosition")]
    public class XmlPreviousPositionComponent : XmlComponentTemplate, IPreviousPositionComponentTemplate
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
            return PreviousPositionComponent.Create(X, Y);
        }

        #endregion Public Methods
    }

    [XmlRoot("Position")]
    public class XmlPositionComponent : XmlComponentTemplate, IPositionComponentTemplate
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
            return PositionComponent.Create(X, Y);
        }

        #endregion Public Methods
    }
}