using OpenTK;

namespace OpenBreed.Game.Common.Components
{
    /// <summary>
    /// Dynamic position entity component class that can be used to store entity current and old position information
    /// Example: Actor is walking with some velocity somewhere in the world at current position
    /// Difference between current and old position will be the velocity
    /// </summary>
    public class DynamicPosition : Position
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with passed initial position value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        public DynamicPosition(Vector2 value) : base(value)
        {
            Old = Current;
        }

        /// <summary>
        /// Constructor with passed initial position values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        public DynamicPosition(float x, float y) : base(x, y)
        {
            Old = Current;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Old position value
        /// </summary>
        public Vector2 Old { get; set; }

        /// <summary>
        /// Velocity value calculated from current and old position difference
        /// </summary>
        public Vector2 Velocity { get { return Vector2.Subtract(Current, Old); } }

        #endregion Public Properties
    }
}