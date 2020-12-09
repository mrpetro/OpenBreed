using OpenBreed.Core.Components.Xml;
using OpenBreed.Core.Modules.Physics.Components;
using System.Xml.Serialization;

namespace OpenBreed.Core.Modules.Rendering.Components.Xml
{
    [XmlRoot("Body")]
    public class XmlBodyComponent : XmlComponentTemplate, IBodyComponentTemplate
    {
        #region Public Properties

        [XmlElement("CofFactor")]
        public float CofFactor { get; set; }

        [XmlElement("CorFactor")]
        public float CorFactor { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Fixtures")]
        [XmlArrayItem(ElementName = "Fixture")]
        public string[] Fixtures { get; set; }

        #endregion Public Properties
    }
}