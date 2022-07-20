using OpenBreed.Core;
using OpenBreed.Wecs.Events;
using System;

namespace OpenBreed.Wecs.Systems.Core.Events
{
    public class WorldUnpausedEventArgs : EntityEvent
    {
        #region Public Constructors

        public WorldUnpausedEventArgs(int entityId, int worldId)
            : base(entityId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}