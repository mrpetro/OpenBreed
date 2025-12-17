namespace OpenBreed.Wecs.Abstractions.Services
{
    public delegate void EntityOnTriggerActionCallback(IEntity actorEntity, IEntity triggerEntity);

    public interface IEntityTriggerMan
    {
        #region Public Methods

        void RegisterCallback(string triggerName, string actionName, EntityOnTriggerActionCallback callback);

        bool TryGetCallback(string triggerName, string actionName, out EntityOnTriggerActionCallback callback);

        #endregion Public Methods
    }
}