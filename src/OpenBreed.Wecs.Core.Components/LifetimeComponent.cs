namespace OpenBreed.Wecs.Core.Components
{
    public interface ILifetimeComponentTemplate : IComponentTemplate
    {
        float TimeLeft { get; }
    }

    /// <summary>
    /// Component
    /// </summary>
    [ComponentName("Lifetime")]
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
}