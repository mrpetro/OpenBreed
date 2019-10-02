using OpenTK;
using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public class Direction : IDirection
    {
        private Vector2 value;

        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial direction value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Direction(Vector2 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Constructor with passed initial direction values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private Direction(float x, float y)
        {
            this.value = new Vector2(x, y);
        }

        #endregion Private Constructors

        #region Public Events

        public event EventHandler<Vector2> ValueChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 Value
        {
            get { return value; }
            set
            {
                if (this.value == value)
                    return;

                this.value = value;
                ValueChanged?.Invoke(this, this.value);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public static IDirection Create(Vector2 value)
        {
            return new Direction(value);
        }

        public static IDirection Create(float x, float y)
        {
            return new Direction(x, y);
        }

        #endregion Public Methods
    }
}