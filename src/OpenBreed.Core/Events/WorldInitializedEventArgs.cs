using OpenBreed.Core;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when world is initialized
    /// </summary>
    public class WorldInitializedEventArgs : EventArgs
    {
        #region Public Constructors

        public WorldInitializedEventArgs(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}