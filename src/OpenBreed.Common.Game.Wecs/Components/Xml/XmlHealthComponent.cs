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
    [XmlRoot("Health")]
    public class XmlHealthComponent : XmlComponentTemplate, IHealthComponentTemplate
    {
        #region Public Properties

        [XmlElement("MaximumValue")]
        public int MaximumRoundsCount { get; set; }

        [XmlElement("Value")]
        public int RoundsCount { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new HealthComponent(
                MaximumRoundsCount,
                RoundsCount);
        }

        #endregion Public Methods
    }
}