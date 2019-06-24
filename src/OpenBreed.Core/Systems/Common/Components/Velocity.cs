using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Velocity entity component class that can be used to store entity current velocity information
    /// Example: Actor is going somewhere with specific velocity vector
    /// </summary>
    public class Velocity : IVelocity
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with passed initial velocity value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Velocity(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial velocity values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private Velocity(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Velocity value
        /// </summary>
        public Vector2 Value { get; set; }

        /// <summary>
        /// System type that this component is part of
        /// </summary>
        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public static IVelocity Create(Vector2 value)
        {
            return new Velocity(value);
        }

        public static IVelocity Create(float x, float y)
        {
            return new Velocity(x, y);
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