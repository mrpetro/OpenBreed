using System;

namespace OpenBreed.Wecs.Events
{
    /// <summary>
    /// Event args for event that occurs when is added to world
    /// </summary>
    public class EntityEnteredEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityEnteredEventArgs(int worldId, int entityId)
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