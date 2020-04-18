using OpenBreed.Core.Common;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when world is deinitialized
    /// </summary>
    public class WorldDeinitializedEventArgs : EventArgs
    {
        #region Public Constructors

        public WorldDeinitializedEventArgs(World world)
        {
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public World World { get; }

        #endregion Public Properties
    }
}