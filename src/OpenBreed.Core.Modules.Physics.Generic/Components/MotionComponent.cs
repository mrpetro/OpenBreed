using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Physics.Components
{
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
}