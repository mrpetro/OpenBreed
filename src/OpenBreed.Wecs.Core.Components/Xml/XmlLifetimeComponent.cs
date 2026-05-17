using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("Lifetime")]
    public class XmlLifetimeComponent : XmlComponentTemplate, ILifetimeComponentTemplate
    {
        #region Public Properties

        [XmlElement("TimeLeft")]
        public float TimeLeft { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new LifetimeComponent(TimeLeft);
        }

        #endregion Public Methods
    }
}