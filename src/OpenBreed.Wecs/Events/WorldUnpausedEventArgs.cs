using OpenBreed.Core;
using System;

namespace OpenBreed.Wecs.Events
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