using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    /// <summary>
    /// Event args for event that occurs every update
    /// </summary>
    public class WorldUpdateEvent : EventArgs
    {
        #region Public Constructors

        public WorldUpdateEvent(int worldId)
        {
            WorldId = worldId;
        }

        public int WorldId { get; }

        #endregion Public Constructors
    }
}