using OpenBreed.Wecs.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenBreed.Wecs.Components.Xml;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Lives")]
    public class XmlLivesComponent : XmlComponentTemplate, ILivesComponentTemplate
    {
        #region Public Properties

        [XmlElement("Value")]
        public int Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new LivesComponent(Value);
        }

        #endregion Public Methods
    }
}