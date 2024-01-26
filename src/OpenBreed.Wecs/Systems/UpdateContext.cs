using System;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems
{
    internal class UpdateContext : IUpdateContext
    {
        #region Public Constructors

        public UpdateContext(IWorld world)
        {
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Dt { get; private set; }

        public float DtMultiplier { get; internal set; } = 1.0f;

        public bool Paused { get; set; }

        public IWorld World { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void UpdateDeltaTime(float dt)
        {
            //NOTE: DT hoops limiter
            //if (dt > 0.1f)
            //    dt = 0.1f;

            Dt = dt * DtMultiplier;
        }

        #endregion Internal Methods
    }
}