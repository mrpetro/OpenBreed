namespace OpenBreed.Wecs.Abstractions.Systems
{
    /// <summary>
    /// System for callback action when an entity triggered by another entity
    /// </summary>
    public interface IEntityOnTriggerActionSystem : ISystem
    {
        #region Public Properties

        /// <summary>
        /// Name of trigger
        /// </summary>
        string TriggerName { get; }

        /// <summary>
        /// Name of callback action
        /// </summary>
        string ActionName { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Called when entity triggered another entity.
        /// </summary>
        /// <param name="triggeringEntity">Triggering entity that triggered another entity.</param>
        /// <param name="triggerEntity">Entity that is triggered.</param>
        void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity);

        #endregion Public Methods
    }
}