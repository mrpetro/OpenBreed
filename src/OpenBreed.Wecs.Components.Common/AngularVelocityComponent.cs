using OpenBreed.Wecs.Components;
using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IAngularPositionTargetComponentTemplate : IComponentTemplate
    {
        float Value { get; }
    }

    public class AngularPositionTargetComponent : IEntityComponent
    {
        #region Public Constructors

        public AngularPositionTargetComponent(float angleValue)
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


    public sealed class AngularPositionTargetComponentFactory : ComponentFactoryBase<IAngularPositionTargetComponentTemplate>
    {
        public AngularPositionTargetComponentFactory()
        {

        }

        protected override IEntityComponent Create(IAngularPositionTargetComponentTemplate template)
        {
            return new AngularPositionTargetComponent(template.Value);
        }
    }
}