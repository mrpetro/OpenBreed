using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event args for event that occurs when is removed from world
    /// </summary>
    public class EntityRemovedEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityRemovedEventArgs(int worldId, int entityId)
        {
            WorldId = worldId;
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public int EntityId { get; }

        #endregion Public Properties
    }
}