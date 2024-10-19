using OpenBreed.Wecs.Attributes;

namespace OpenBreed.Wecs.Components.Common
{
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

        public MetadataComponent(string level, string name, string option, string flavor)
        {
            Level = level;
            Name = name;
            Option = option;
            Flavor = flavor;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Level { get; }
        public string Name { get; }
        public string Option { get; }
        public string Flavor { get; set; }
        public string State { get; set; }

        #endregion Public Properties
    }

    public sealed class MetadataComponentFactory : ComponentFactoryBase<IMetadataComponentTemplate>
    {
        #region Internal Constructors

        public MetadataComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IMetadataComponentTemplate template)
        {
            return new MetadataComponent(template.Level, template.Name, template.Option, template.Flavor);
        }

        #endregion Protected Methods
    }
}