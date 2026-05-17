namespace OpenBreed.Wecs.Core.Components
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
}