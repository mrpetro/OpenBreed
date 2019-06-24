using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public class Direction : IDirection
    {
        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial direction value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Direction(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial direction values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private Direction(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 Value { get; set; }

        /// <summary>
        /// System type that this component is part of
        /// </summary>
        public Type SystemType { get { return null; } }

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

        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Initialize(IEntity entity)
        {
        }

        /// <summary>
        /// Deinitialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        public void Deinitialize(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}