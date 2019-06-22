using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Systems.Common.Components
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
        public Thrust(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial thrust values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        public Thrust(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Thrust value
        /// </summary>
        public Vector2 Value { get; set; }

        /// <summary>
        /// System type that this component is part of
        /// </summary>
        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

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