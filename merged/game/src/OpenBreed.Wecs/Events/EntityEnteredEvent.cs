using System;

namespace OpenBreed.Wecs.Events
{
    /// <summary>
    /// Event args for event that occurs when is added to world
    /// </summary>
    public class EntityEnteredEvent : EntityEvent
    {
        #region Public Constructors

        public EntityEnteredEvent(int entityId, int worldId)
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