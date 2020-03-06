using OpenTK;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Position entity component class that can be used to store entity current position information
    /// Example: Actor is standing somewhere in the world at current position
    /// </summary>
    public class PositionComponent : IEntityComponent
    {
        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial position values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private PositionComponent(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        /// <summary>
        /// Constructor with passed initial position value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private PositionComponent(Vector2 value)
        {
            Value = value;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Position value
        /// </summary>
        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static PositionComponent Create(Vector2 value)
        {
            return new PositionComponent(value);
        }

        public static PositionComponent Create(float x, float y)
        {
            return new PositionComponent(x, y);
        }

        #endregion Public Methods
    }
}