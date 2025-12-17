namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event fired when entity is leaving the world
    /// </summary>
    public class EntityLeavingEvent : EntityEvent
    {
        #region Public Constructors

        public EntityLeavingEvent(int entityId, int worldId)
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