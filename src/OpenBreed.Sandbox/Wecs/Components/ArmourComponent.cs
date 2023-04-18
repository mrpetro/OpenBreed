using OpenBreed.Wecs.Components;

namespace OpenBreed.Sandbox.Wecs.Components
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

    public sealed class ArmourComponentFactory : ComponentFactoryBase<IArmourComponentTemplate>
    {
        #region Public Constructors

        public ArmourComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IArmourComponentTemplate template)
        {
            return new ArmourComponent(
                template.Value);
        }

        #endregion Protected Methods
    }
}