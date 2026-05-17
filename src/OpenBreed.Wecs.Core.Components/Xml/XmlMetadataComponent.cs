using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("Metadata")]
    public class XmlMetadataComponent : XmlComponentTemplate, IMetadataComponentTemplate
    {
        #region Public Properties

        [XmlElement("Level")]
        public string Level { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Option")]
        public string Option { get; set; }

        [XmlElement("Flavor")]
        public string Flavor { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new MetadataComponent(Level, Name, Option, Flavor);
        }

        #endregion Public Methods
    }
}