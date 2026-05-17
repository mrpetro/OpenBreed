using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Keys")]
    public class XmlKeysComponent : XmlComponentTemplate, IKeysComponentTemplate
    {
        #region Public Properties

        [XmlElement("GeneralCount")]
        public int GeneralCount { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new KeysComponent(GeneralCount);
        }

        #endregion Public Methods
    }
}