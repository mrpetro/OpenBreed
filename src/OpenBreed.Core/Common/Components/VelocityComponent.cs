using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Velocity entity component class that can be used to store entity current velocity information
    /// Example: Actor is going somewhere with specific velocity vector
    /// </summary>
    public class VelocityComponent : IEntityComponent
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with passed initial velocity value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private VelocityComponent(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial velocity values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private VelocityComponent(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Velocity value
        /// </summary>
        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static VelocityComponent Create(Vector2 value)
        {
            return new VelocityComponent(value);
        }

        public static VelocityComponent Create(float x, float y)
        {
            return new VelocityComponent(x, y);
        }

        #endregion Public Methods
    }
}