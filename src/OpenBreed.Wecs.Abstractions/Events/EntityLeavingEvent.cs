namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when entity is leaving the world.
    /// </summary>
    public class EntityLeavingEvent : WorldEvent
    {
        #region Public Constructors

        public EntityLeavingEvent(int worldId, int entityId)
            : base(worldId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }

        #endregion Public Properties
    }
}