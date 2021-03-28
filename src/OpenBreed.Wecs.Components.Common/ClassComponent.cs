namespace OpenBreed.Wecs.Components.Common
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string Name { get; }

        #endregion Public Properties
    }

    public class ClassComponent : IEntityComponent
    {
        //public ClassComponent(ClassComponentBuilder builder)
        //{
        //    Name = builder.Name;
        //}

        #region Public Constructors

        public ClassComponent(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

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
            return new ClassComponent(template.Name);
        }

        #endregion Protected Methods
    }
}