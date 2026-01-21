using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Services
{
    internal class EntityTriggerMan : IEntityTriggerMan
    {
        #region Private Fields
    
        private readonly Dictionary<string, Dictionary<string, IActionOnTriggerSystem>> actionOnTriggerSystemLookup = new Dictionary<string, Dictionary<string, IActionOnTriggerSystem>>();

        #endregion Private Fields

        #region Public Methods

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, IActionOnTriggerSystem
        {
            if (!actionOnTriggerSystemLookup.TryGetValue(system.TriggerName, out var actionLookup))
            {
                actionLookup = new Dictionary<string, IActionOnTriggerSystem>();
                actionOnTriggerSystemLookup.Add(system.TriggerName, actionLookup);
            }

            if (!actionLookup.TryAdd(system.ActionName, system))
            {
                throw new InvalidOperationException($"Action '{system.ActionName}' is already registered for trigger '{system.TriggerName}'.");
            }
        }

        public bool TryGetTriggerSystem<TSystem>(string triggerName, string actionName, out TSystem system) where TSystem : class, IActionOnTriggerSystem
        {
            if (!actionOnTriggerSystemLookup.TryGetValue(triggerName, out var actionLookup))
            {
                system = default;
                return false;
            }

            if (!actionLookup.TryGetValue(actionName, out IActionOnTriggerSystem actionOnTriggerSystem))
            {
                system = default;
                return false;
            }

            system = actionOnTriggerSystem as TSystem;
            return system is not null;
        }

        #endregion Public Methods
    }
}