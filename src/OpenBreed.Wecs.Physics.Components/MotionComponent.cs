using OpenBreed.Wecs.Abstractions.Services;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Physics.Components
{
    public interface IMotionComponentTemplate : IComponentTemplate
    {
    }

    public class MotionComponent : IEntityComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private readonly float MAX_SPEED = 120.0f;

        #endregion Private Fields

        #region Public Constructors

        public MotionComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public float SpeedPercent
        {
            get
            {
                return speedPercent;
            }

            set
            {
                speedPercent = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }

        public float Acceleration { get { return speedPercent * MAX_SPEED; } }

        #endregion Public Properties
    }
}