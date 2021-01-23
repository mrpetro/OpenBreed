using OpenBreed.Components.Common.Xml;
using OpenBreed.Ecsw.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Components.Physics.Xml
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