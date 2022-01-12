using OpenBreed.Wecs.Components;
using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IAngularVelocityComponentTemplate : IComponentTemplate
    {
        float Value { get; }
    }

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


    public sealed class AngularVelocityComponentFactory : ComponentFactoryBase<IAngularVelocityComponentTemplate>
    {
        public AngularVelocityComponentFactory()
        {

        }

        protected override IEntityComponent Create(IAngularVelocityComponentTemplate template)
        {
            return new AngularVelocityComponent(template.Value);
        }
    }
}