namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Event that occurs when entity left the world.
    /// </summary>
    public class EntityLeftEvent : WorldEvent, IEntityEvent
    {
        #region Public Constructors

        public EntityLeftEvent(int worldId, int entityId)
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