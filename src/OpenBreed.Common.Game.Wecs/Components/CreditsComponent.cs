using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface ICreditsComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }

        #endregion Public Properties
    }

    public class CreditsComponent : IEntityComponent
    {
        public CreditsComponent(int value)
        {
            Value = value;
        }

        #region Public Properties

        public int Value { get; set; }

        #endregion Public Properties
    }
}
