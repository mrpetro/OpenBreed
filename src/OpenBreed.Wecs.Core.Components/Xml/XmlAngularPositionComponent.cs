using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("AngularPosition")]
    public class XmlAngularPositionComponent : XmlComponentTemplate, IAngularPositionComponentTemplate
    {
        #region Public Properties

        [XmlElement("Value")]
        public float Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new AngularPositionComponent(Value);
        }

        #endregion Public Methods
    }
}