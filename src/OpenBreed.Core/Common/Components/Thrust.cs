using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Thrust entity component class that can be used to store entity current thrust information
    /// Example: Actor is applied with specific thrust vector to move in specific direction
    /// </summary>
    public class Thrust : IEntityComponent
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with passed initial thrust value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Thrust(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial thrust values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private Thrust(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Thrust value
        /// </summary>
        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Thrust Create(Vector2 value)
        {
            return new Thrust(value);
        }

        public static Thrust Create(float x, float y)
        {
            return new Thrust(x, y);
        }

        #endregion Public Methods
    }
}