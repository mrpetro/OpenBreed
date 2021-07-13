using OpenTK;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IThrustComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float X { get; }
        float Y { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Thrust entity component class that can be used to store entity current thrust information
    /// Example: Actor is applied with specific thrust vector to move in specific direction
    /// </summary>
    public class ThrustComponent : IEntityComponent
    {
        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial thrust value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private ThrustComponent(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial thrust values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private ThrustComponent(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Thrust value
        /// </summary>
        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static ThrustComponent Create(Vector2 value)
        {
            return new ThrustComponent(value);
        }

        public static ThrustComponent Create(float x, float y)
        {
            return new ThrustComponent(x, y);
        }

        #endregion Public Methods
    }

    public sealed class ThrustComponentFactory : ComponentFactoryBase<IThrustComponentTemplate>
    {
        #region Internal Constructors

        internal ThrustComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IThrustComponentTemplate template)
        {
            return ThrustComponent.Create(template.X, template.Y);
        }

        #endregion Protected Methods
    }
}