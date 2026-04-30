namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when entity entered the world.
    /// </summary>
    public class EntityEnteredEvent : WorldEvent, IEntityEvent
    {
        #region Public Constructors

        public EntityEnteredEvent(int worldId, int entityId)
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