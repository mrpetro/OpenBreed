namespace OpenBreed.Wecs.Components.Common
{
    public interface ILifetimeComponentTemplate : IComponentTemplate
    {
        float TimeLeft { get; }
    }

    /// <summary>
    /// Component
    /// </summary>
    public class LifetimeComponent : IEntityComponent
    {
        #region Public Constructors

        public LifetimeComponent(float timeLeft)
        {
            TimeLeft = timeLeft;
        }

        #endregion Public Constructors

        #region Public Properties

        public float TimeLeft { get; set; }

        #endregion Public Properties
    }

    public sealed class LifetimeComponentFactory : ComponentFactoryBase<ILifetimeComponentTemplate>
    {
        public LifetimeComponentFactory()
        {

        }

        protected override IEntityComponent Create(ILifetimeComponentTemplate template)
        {
            return new LifetimeComponent(template.TimeLeft);
        }
    }

}