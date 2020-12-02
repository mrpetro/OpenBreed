using OpenBreed.Core;
using System;

namespace OpenBreed.Core.Events
{
    public class WorldPausedEventArgs : EventArgs
    {
        #region Private Constructors

        public WorldPausedEventArgs(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Private Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}