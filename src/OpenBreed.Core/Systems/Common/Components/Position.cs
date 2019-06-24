using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Position entity component class that can be used to store entity current position information
    /// Example: Actor is standing somewhere in the world at current position
    /// </summary>
    public struct Position : IPosition
    {
        #region Private Constructors

        /// <summary>
        /// Constructor with passed initial position value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        private Position(Vector2 value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor with passed initial position values
        /// </summary>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        private Position(float x, float y)
        {
            Value = new Vector2(x, y);
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Position value
        /// </summary>
        public Vector2 Value { get; set; }

        /// <summary>
        /// System type that this component is part of
        /// </summary>
        public Type SystemType { get { return null; } }

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