using OpenBreed.Wecs.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Physics.Xml
{
    public class XmlBodyFixtureTemplate : IBodyFixtureTemplate
    {
        [XmlElement("ShapeName")]
        public string ShapeName { get; set; }

        [XmlIgnore]
        public IEnumerable<string> Groups => XmlGroups.Cast<string>();

        [XmlArray("Groups")]
        [XmlArrayItem(ElementName = "Group")]
        public string[] XmlGroups { get; set; }

    }

    [XmlRoot("Body")]
    public class XmlBodyComponent : XmlComponentTemplate, IBodyComponentTemplate
    {
        #region Public Properties

        [XmlElement("CofFactor")]
        public float CofFactor { get; set; }

        [XmlElement("CorFactor")]
        public float CorFactor { get; set; }

        [XmlIgnore]
        public IEnumerable<IBodyFixtureTemplate> Fixtures => XmlFixtures.Cast<IBodyFixtureTemplate>();

        [XmlArray("Fixtures")]
        [XmlArrayItem(ElementName = "Fixture")]
        public XmlBodyFixtureTemplate[] XmlFixtures { get; set; }

        #endregion Public Properties
    }
}