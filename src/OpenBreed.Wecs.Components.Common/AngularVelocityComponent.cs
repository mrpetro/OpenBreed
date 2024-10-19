using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IAAngularVelocityComponentTemplate : IComponentTemplate
    {
        float Value { get; }
    }

    [ComponentName("AngularVelocity")]
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


    public sealed class AngularVelocityComponentFactory : ComponentFactoryBase<IAAngularVelocityComponentTemplate>
    {
        public AngularVelocityComponentFactory()
        {

        }

        protected override IEntityComponent Create(IAAngularVelocityComponentTemplate template)
        {
            return new AngularVelocityComponent(template.Value);
        }
    }
}