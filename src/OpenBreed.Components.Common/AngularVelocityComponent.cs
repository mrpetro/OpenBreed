using OpenBreed.Ecsw.Components;
using OpenTK;
using System;

namespace OpenBreed.Components.Common
{
    public class AngularVelocityComponent : IEntityComponent
    {
        #region Public Constructors

        public AngularVelocityComponent(float value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Direction value
        /// </summary>
        public void SetDirection(Vector2 vector)
        {
            Value = (float)Math.Atan2((float)vector.Y, (float)vector.X);
        }

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 GetDirection()
        {
            return new Vector2((float)Math.Cos(Value), (float)Math.Sin(Value));
        }

        #endregion Public Methods
    }
}