using OpenBreed.Core.Common.Builders;

namespace OpenBreed.Core.Common.Components
{
    public class ClassComponent : IEntityComponent
    {
        #region Public Constructors

        public ClassComponent(ClassComponentBuilder builder)
        {
            Name = builder.Name;
        }

        public ClassComponent(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties
    }
}