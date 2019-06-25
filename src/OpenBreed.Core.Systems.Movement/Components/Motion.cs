using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Movement;
using OpenBreed.Core.Systems.Movement.Components;
using OpenBreed.Core.Systems.Movement.Systems;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Core.Systems.Movement.Components
{
    public class Motion : IEntityComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private float MAXSPEED = 8.0f;

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

        public float Speed { get { return speedPercent * MAXSPEED; } }

        #endregion Public Properties
    }
}