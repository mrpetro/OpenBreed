using System;

namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event args for event that occurs every update
    /// </summary>
    public class EntityFrameEvent : EventArgs
    {
        #region Public Constructors

        public EntityFrameEvent(int entityId)
        {
            EntityId = entityId;
        }

        public int EntityId { get; }

        #endregion Public Constructors
    }
}