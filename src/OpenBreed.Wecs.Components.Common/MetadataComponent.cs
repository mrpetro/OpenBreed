namespace OpenBreed.Wecs.Components.Common
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string Level { get; }
        string Name { get; }
        string Flavor { get; }

        #endregion Public Properties
    }

    public class MetadataComponent : IEntityComponent
    {
        #region Public Constructors

        public MetadataComponent(string level, string name, string flavor)
        {
            Level = level;
            Name = name;
            Flavor = flavor;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Level { get; }
        public string Name { get; }
        public string Flavor { get; }

        #endregion Public Properties
    }

    public sealed class ClassComponentFactory : ComponentFactoryBase<IClassComponentTemplate>
    {
        #region Internal Constructors

        internal ClassComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IClassComponentTemplate template)
        {
            return new MetadataComponent(template.Level, template.Name, template.Flavor);
        }

        #endregion Protected Methods
    }
}