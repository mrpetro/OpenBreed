namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Core related event types
    /// </summary>
    public static class CoreEventTypes
    {
        #region Public Fields

        /// <summary>
        /// Occurs when world is initialized
        /// </summary>
        public const string WORLD_INITIALIZED = "WORLD_INITIALIZED";

        /// <summary>
        /// Occurs when world is deinitialized
        /// </summary>
        public const string WORLD_DEINITIALIZED = "WORLD_DEINITIALIZED";

        /// <summary>
        /// Occurs when entity is removed from world
        /// </summary>
        public const string ENTITY_LEFT_WORLD = "ENTITY_REMOVED_FROM_WORLD";

        /// <summary>
        /// Occurs when entity is added to world
        /// </summary>
        public const string ENTITY_ENTERED_WORLD = "ENTITY_ADDED_TO_WORLD";

        #endregion Public Fields
    }
}