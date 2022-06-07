using System;

namespace OpenBreed.Wecs.Worlds
{
    internal class WorldContext : IWorldContext
    {
        #region Public Constructors

        public WorldContext(World world)
        {
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Dt { get; private set; }

        public float DtMultiplier { get; internal set; } = 1.0f;

        public bool Paused { get; set; }

        public World World { get; }

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