using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Services
{
    public delegate void EntityOnTriggerActionCallback(IEntity actorEntity, IEntity triggerEntity);

    public interface IEntityTriggerMan
    {
        void RegisterCallback(string triggerName, string actionName, EntityOnTriggerActionCallback callback);
        bool TryGetCallback(string triggerName, string actionName, out EntityOnTriggerActionCallback callback);
    }
}