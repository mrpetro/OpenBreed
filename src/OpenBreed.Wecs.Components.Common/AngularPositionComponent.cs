using OpenBreed.Core;
using OpenBreed.Wecs.Components;
using OpenTK;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IAngularPositionComponentTemplate : IComponentTemplate
    {
        float Value { get; }
    }

    /// <summary>
    /// Direction entity component class that can be used to store entity current direction information
    /// Example: Actor is facing particular direction when standing
    /// </summary>
    public class AngularPositionComponent : IEntityComponent
    {
        #region Internal Constructors

        /// <summary>
        /// Constructor with passed initial direction value
        /// </summary>
        /// <param name="value">Initial value vector</param>
        //internal AngularPositionComponent(AngularPositionComponentBuilder builder)
        //{
        //    Value = builder.Value;
        //}

        internal AngularPositionComponent(float value)
        {
            Value = value;
        }

        #endregion Internal Constructors

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


    public sealed class AngularPositionComponentFactory : ComponentFactoryBase<IAngularPositionComponentTemplate>
    {
        internal AngularPositionComponentFactory()
        {

        }

        protected override IEntityComponent Create(IAngularPositionComponentTemplate template)
        {
            return new AngularPositionComponent(template.Value);
        }
    }
}