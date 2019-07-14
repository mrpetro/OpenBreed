using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Movement;
using OpenBreed.Core.Modules.Animation.Systems.Movement.Components;
using OpenBreed.Core.Modules.Animation.Systems.Movement.Systems;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems.Movement.Components
{
    public class Motion : IEntityComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private float MAX_ACCELERATION = 1024.0f;

        #endregion Private Fields

        #region Public Constructors

        public Motion()
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