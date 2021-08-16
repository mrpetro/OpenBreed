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
        /// <param name="angleValue">Initial value vector</param>
        //internal AngularPositionComponent(AngularPositionComponentBuilder builder)
        //{
        //    Value = builder.Value;
        //}

        internal AngularPositionComponent(float angleValue)
        {
            Value = GetDirection(angleValue);
        }

        #endregion Internal Constructors

        #region Public Properties

        public Vector2 Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Direction value
        /// </summary>
        public Vector2 GetDirection(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
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