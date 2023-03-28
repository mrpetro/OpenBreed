using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Components.Common;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface IHealthComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Current { get; }
        int Maximum { get; }

        #endregion Public Properties
    }

    public class HealthComponent : IEntityComponent
    {
        public HealthComponent(
            int maximum,
            int current)
        {
            Maximum = maximum;
            Current = current;
        }

        #region Public Properties

        public int Maximum { get; set; }
        public int Current { get; set; }

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
                template.Maximum,
                template.Current);
        }

        #endregion Protected Methods
    }
}