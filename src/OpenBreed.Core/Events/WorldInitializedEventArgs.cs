using OpenBreed.Core.Common;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event arguments that are passed with WORLD_INITIALIZED event
    /// </summary>
    public class WorldInitializedEventArgs : EventArgs
    {
        #region Public Constructors

        public WorldInitializedEventArgs(World world)
        {
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public World World { get; }

        #endregion Public Properties
    }
}