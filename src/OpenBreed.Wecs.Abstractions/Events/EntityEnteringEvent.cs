namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when entity is entering the world.
    /// </summary>
    public class EntityEnteringEvent : WorldEvent, IEntityEvent
    {
        #region Public Constructors

        public EntityEnteringEvent(int worldId, int entityId)
            : base(worldId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// ID if entity that is entering the world.
        /// </summary>
        public int EntityId { get; }

        #endregion Public Properties
    }
}