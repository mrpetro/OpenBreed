using OpenBreed.Wecs.Abstractions.Systems;
using System;

namespace OpenBreed.Wecs.Abstractions.Services
{    
    public interface IEntityTriggerMan
    {
        #region Public Methods

        bool TryGetTriggerSystem<TSystem>(string triggerName, string actionName, out TSystem system) where TSystem : class, IActionOnTriggerSystem;

        void RegisterSystem<TSystem>(TSystem system) where TSystem : class, IActionOnTriggerSystem;


        #endregion Public Methods
    }
}