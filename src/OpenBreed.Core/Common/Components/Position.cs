using OpenTK;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Position entity component class that can be used to store entity current position information
    /// Example: Actor is standing somewhere in the world at current position
    /// </summary>
    public class Position : IPosition
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with passed initial position values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        public Position(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Public Constructors

        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial position value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Position(Vector2 value)
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

        public static IPosition Create(Vector2 value)
        {
            return new Position(value);
        }

        public static IPosition Create(float x, float y)
        {
            return new Position(x, y);
        }

        #endregion Public Methods
    }
}