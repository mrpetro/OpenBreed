namespace OpenBreed.Core.Systems.Control.Events
{
    /// <summary>
    /// Control related event types
    /// </summary>
    public static class ControlEventTypes
    {
        #region Public Fields

        /// <summary>
        /// Occurs when control direction vector changes
        /// </summary>
        public const string CONTROL_DIRECTION_CHANGED = "CONTROL_DIRECTION_CHANGED";

        /// <summary>
        /// Occurs wen control fire flag changes
        /// </summary>
        public const string CONTROL_FIRE_CHANGED = "CONTROL_FIRE_CHANGED";

        #endregion Public Fields
    }
}