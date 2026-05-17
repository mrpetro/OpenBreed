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
    [XmlRoot("Damager")]
    public class XmlDamagerComponent : XmlComponentTemplate, IDamagerComponentTemplate
    {
        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new DamagerComponent();
        }

        #endregion Public Methods
    }
}