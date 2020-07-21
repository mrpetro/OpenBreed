
namespace OpenBreed.Core.Common.Components
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
    }
}