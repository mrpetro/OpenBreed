using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IArmourComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }

        #endregion Public Properties
    }

    public class ArmourComponent : IEntityComponent
    {
        #region Public Constructors

        public ArmourComponent(int value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Value { get; set; }

        #endregion Public Properties
    }
}