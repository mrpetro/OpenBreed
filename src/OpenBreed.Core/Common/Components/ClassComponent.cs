using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    public class ClassComponent : IEntityComponent
    {
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
}