using OpenBreed.Core.Builders;

namespace OpenBreed.Core.Components
{
    public interface IClassComponentTemplate : IComponentTemplate
    {
        string Name { get; }
    }

    public class ClassComponent : IEntityComponent
    {
        #region Public Constructors

        //public ClassComponent(ClassComponentBuilder builder)
        //{
        //    Name = builder.Name;
        //}

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
        public ClassComponentFactory(ICore core) : base(core)
        {

        }

        protected override IEntityComponent Create(IClassComponentTemplate template)
        {
            return new ClassComponent(template.Name);
        }
    }
}