using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Core.Blueprints
{
    [XmlRoot("Blueprint")]
    public class BlueprintXml : IBlueprint
    {
        #region Public Properties

        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public List<IEntityDef> Entities { get { return EntitiesXml.Cast<IEntityDef>().ToList(); } }

        [XmlArray("Entities")]
        [XmlArrayItem("Entity")]
        public List<EntityDefXml> EntitiesXml { get; } = new List<EntityDefXml>();

        #endregion Public Properties
    }
}