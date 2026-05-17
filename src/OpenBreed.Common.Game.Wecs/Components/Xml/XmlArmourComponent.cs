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
    [XmlRoot("Armour")]
    public class XmlArmourComponent : XmlComponentTemplate, IArmourComponentTemplate
    {
        #region Public Properties

        [XmlElement("Value")]
        public int Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new ArmourComponent(
                Value);
        }

        #endregion Public Methods
    }
}