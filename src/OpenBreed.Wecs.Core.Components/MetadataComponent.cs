using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Core.Components
{
    public interface IMetadataAttributeTemplate
    {
        string Name { get; }

        object ValueObject { get; }
    }

    public interface IMetadataAttributeTemplate<TValue> : IMetadataAttributeTemplate
    {
        TValue Value { get; }
    }

    public interface IMetadataComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string Level { get; }
        string Name { get; }
        string Option { get; }
        string Flavor { get; }

        #endregion Public Properties
    }

    [ComponentName("Metadata")]
    public class MetadataComponent : IEntityComponent
    {
        #region Public Constructors

        public MetadataComponent(string level, string name, string option, string flavor, IEnumerable<IMetadataAttributeTemplate> attributes)
        {
            Level = level;
            Name = name;
            Option = option;
            Flavor = flavor;
            Attributes = attributes.ToDictionary(item => item.Name, item => item.ValueObject);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Level { get; }
        public string Name { get; }
        public string Option { get; }
        public string Flavor { get; set; }
        public string State { get; set; }

        public Dictionary<string, object> Attributes { get; }

        #endregion Public Properties
    }
}