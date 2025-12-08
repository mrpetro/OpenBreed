using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Services
{
    internal class EntityTriggerMan : IEntityTriggerMan
    {
        #region Private Fields

        private readonly Dictionary<string, Dictionary<string, EntityOnTriggerActionCallback>> triggerActionLookup = new Dictionary<string, Dictionary<string, EntityOnTriggerActionCallback>>();

        #endregion Private Fields

        #region Public Methods

        public void RegisterCallback(string triggerName, string actionName, EntityOnTriggerActionCallback callback)
        {
            if (!triggerActionLookup.TryGetValue(triggerName, out var actionLookup))
            {
                actionLookup = new Dictionary<string, EntityOnTriggerActionCallback>();
                triggerActionLookup.Add(triggerName, actionLookup);
            }

            if (!actionLookup.TryAdd(actionName, callback))
            {
                throw new InvalidOperationException($"Action '{actionName}' is already registered for trigger '{triggerName}'.");
            }
        }

        public bool TryGetCallback(string triggerName, string actionName, out EntityOnTriggerActionCallback callback)
        {
            if (!triggerActionLookup.TryGetValue(triggerName, out var actionLookup))
            {
                callback = null;
                return false;
            }

            return actionLookup.TryGetValue(actionName, out callback);
        }

        #endregion Public Methods
    }
}