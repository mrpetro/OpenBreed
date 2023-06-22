using System;

namespace OpenBreed.Wecs.Events
{
    /// <summary>
    /// Event args for event that occurs when is left the world
    /// </summary>
    public class EntityLeftEvent : EntityEvent
    {
        #region Public Constructors

        public EntityLeftEvent(int entityId, int worldId)
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