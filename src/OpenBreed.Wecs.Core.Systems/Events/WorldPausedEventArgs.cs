using OpenBreed.Wecs.Abstractions.Events;
using System;

namespace OpenBreed.Wecs.Core.Systems.Events
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