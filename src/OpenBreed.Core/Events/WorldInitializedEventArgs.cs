using OpenBreed.Core.Common;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when world is initialized
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