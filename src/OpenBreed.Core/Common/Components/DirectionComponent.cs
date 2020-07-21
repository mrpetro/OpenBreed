using OpenBreed.Core.Common.Builders;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Components
{
    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public class DirectionComponent : IEntityComponent
    {
        #region Private Fields

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Constructor with passed initial direction value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        internal DirectionComponent(DirectionComponentBuilder builder)
        {
            //Value = builder.Value;

            SetDirection(builder.Value);
        }

        #endregion Internal Constructors

        #region Public Events

        #endregion Public Events

        #region Public Properties

        public float ValueEx { get; set; }

        /// <summary>
        /// Direction value
        /// </summary>
        public void SetDirection(Vector2 vector)
        {
            ValueEx = (float)Math.Atan2((float)vector.Y, (float)vector.X);
        }

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 GetDirection()
        {
            return new Vector2((float)Math.Cos(ValueEx), (float)Math.Sin(ValueEx));
        }

        /// <summary>
        /// Direction value
        /// </summary>
        //public Vector2 Value { get; set; }

        #endregion Public Properties
    }
}