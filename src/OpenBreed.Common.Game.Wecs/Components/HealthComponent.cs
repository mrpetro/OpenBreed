using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Components.Common;

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

    public sealed class HealthComponentFactory : ComponentFactoryBase<IHealthComponentTemplate>
    {
        #region Internal Constructors

        public HealthComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IHealthComponentTemplate template)
        {
            return new HealthComponent(
                template.MaximumRoundsCount,
                template.RoundsCount);
        }

        #endregion Protected Methods
    }
}