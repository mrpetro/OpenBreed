using OpenTK;

namespace OpenBreed.Wecs.Components.Physics
{
    public interface IMotionComponentTemplate : IComponentTemplate
    {
    }

    public class MotionComponent : IEntityComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private float MAX_ACCELERATION = 1024.0f;

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

        public float Acceleration { get { return speedPercent * MAX_ACCELERATION; } }

        #endregion Public Properties
    }

    public sealed class MotionComponentFactory : ComponentFactoryBase<IMotionComponentTemplate>
    {
        #region Internal Constructors

        internal MotionComponentFactory() : base(null)
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IMotionComponentTemplate template)
        {
            return new MotionComponent();
        }

        #endregion Protected Methods
    }
}