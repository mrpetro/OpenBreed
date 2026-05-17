using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Physics.Components.Xml
{
    [XmlRoot("Motion")]
    public class XmlMotionComponent : XmlComponentTemplate, IMotionComponentTemplate
    {
        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new MotionComponent();
        }

        #endregion Public Methods
    }
}