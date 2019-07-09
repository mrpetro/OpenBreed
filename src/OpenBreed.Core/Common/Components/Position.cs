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
        /// Constructor with passed initial position value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        public Position(Vector2 value)
        {
            Value = value;
        }

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

        #region Public Properties

        /// <summary>
        /// Position value
        /// </summary>
        public Vector2 Value { get; set; }

        #endregion Public Properties
    }
}