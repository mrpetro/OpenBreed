using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Common.Game.Wecs.Services
{
    public delegate void ActorTriggerCallback(IEntity actorEntity, IEntity triggerEntity);

    public interface IActorTriggerMan
    {
        void RegisterCallback(string triggerType, ActorTriggerCallback callback);
        bool TryGetCallback(string triggerType, out ActorTriggerCallback callback);
    }
}