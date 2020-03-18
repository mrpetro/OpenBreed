using OpenBreed.Core.Common.Builders;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public class DirectionComponent : IEntityComponent
    {
        #region Private Fields

        private Vector2 value;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Constructor with passed initial direction value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        internal DirectionComponent(DirectionComponentBuilder builder)
        {
            this.value = builder.Value;
        }

        #endregion Internal Constructors

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
    }
}