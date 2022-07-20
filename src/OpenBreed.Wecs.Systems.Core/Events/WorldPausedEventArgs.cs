using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    public class WorldPausedEventArgs : EntityEvent
    {
        #region Private Constructors

        public WorldPausedEventArgs(int entityId, int worldId)
            : base(entityId)
        {
            WorldId = worldId;
        }

        #endregion Private Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}