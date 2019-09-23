using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Core.Blueprints
{
    public class EntityDefXml : IEntityDef
    {
        #region Public Properties

        public string Name { get; set; }

        [XmlArray("ComponentTypes")]
        [XmlArrayItem("ComponentType")]
        public List<string> ComponentTypes { get; } = new List<string>();

        [XmlIgnore]
        public List<IComponentState> ComponentStates { get { return ComponentStatesXml.Cast<IComponentState>().ToList(); } }

        [XmlArray("ComponentStates")]
        [XmlArrayItem("ComponentState")]
        public List<ComponentStateXml> ComponentStatesXml { get; } = new List<ComponentStateXml>();

        #endregion Public Properties
    }
}