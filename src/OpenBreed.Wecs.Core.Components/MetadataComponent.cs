using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Core.Components
{
    public interface IMetadataAttributeTemplate
    {
        #region Public Properties

        string Name { get; }

        object ValueObject { get; }

        #endregion Public Properties
    }

    public interface IMetadataAttributeTemplate<TValue> : IMetadataAttributeTemplate
    {
        #region Public Properties

        TValue Value { get; }

        #endregion Public Properties
    }

    public interface IMetadataComponentTemplate : IComponentTemplate
    {
    }

    [ComponentName("Metadata")]
    public class MetadataComponent : IEntityComponent
    {
        #region Public Constructors

        public MetadataComponent(IEnumerable<IMetadataAttributeTemplate> attributes)
        {
            Attributes = attributes.ToDictionary(item => item.Name, item => item.ValueObject);
        }

        #endregion Public Constructors

        #region Public Properties

        public Dictionary<string, object> Attributes { get; }

        #endregion Public Properties
    }
}