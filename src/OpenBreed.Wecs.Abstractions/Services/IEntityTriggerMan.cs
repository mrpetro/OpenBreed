using OpenBreed.Wecs.Abstractions.Systems;
using System;

namespace OpenBreed.Wecs.Abstractions.Services
{
    public delegate void EntityOnTriggerActionCallback(IEntity actorEntity, IEntity triggerEntity);
      
    public interface IEntityTriggerMan
    {
        #region Public Methods

        void RegisterCallback(string triggerName, string actionName, EntityOnTriggerActionCallback callback);

        bool TryGetCallback(string triggerName, string actionName, out EntityOnTriggerActionCallback callback);

        bool TryGetTriggerSystem<TSystem>(string triggerName, string actionName, out TSystem system) where TSystem : class, IActionOnTriggerSystem;

        void RegisterSystem<TSystem>(TSystem system) where TSystem : class, IActionOnTriggerSystem;


        #endregion Public Methods
    }
}