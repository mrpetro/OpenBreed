using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Core.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IHealthComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int RoundsCount { get; }
        int MaximumRoundsCount { get; }

        #endregion Public Properties
    }

    public class HealthComponent : IEntityComponent
    {
        public HealthComponent(
            int maximumValue,
            int value)
        {
            MaximumValue = maximumValue;
            Value = value;
        }

        #region Public Properties

        public int MaximumValue { get; set; }
        public int Value { get; set; }


        public float GetPercent() => (float)Value / (float)MaximumValue;

        #endregion Public Properties
    }
}