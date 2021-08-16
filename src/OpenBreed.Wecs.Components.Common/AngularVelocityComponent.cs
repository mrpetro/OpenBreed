using OpenBreed.Wecs.Components;
using OpenTK;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public class AngularVelocityComponent : IEntityComponent
    {
        #region Public Constructors

        public AngularVelocityComponent(float angleValue)
        {
            Value = GetDirection(angleValue);
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 GetDirection(float angleValue)
        {
            return new Vector2((float)Math.Cos(angleValue), (float)Math.Sin(angleValue));
        }

        #endregion Public Methods
    }
}