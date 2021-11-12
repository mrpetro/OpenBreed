namespace OpenBreed.Wecs.Components.Common
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string Name { get; }
        string Flavor { get; }

        #endregion Public Properties
    }

    public class ClassComponent : IEntityComponent
    {
        //public ClassComponent(ClassComponentBuilder builder)
        //{
        //    Name = builder.Name;
        //}

        #region Public Constructors

        public ClassComponent(string name, string flavor)
        {
            Name = name;
            Flavor = flavor;
        }

        #endregion Public Constructors

        #region Public Properties

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
            return new ClassComponent(template.Name, template.Flavor);
        }

        #endregion Protected Methods
    }
}