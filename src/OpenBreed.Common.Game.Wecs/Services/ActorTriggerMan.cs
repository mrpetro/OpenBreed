using OpenBreed.Common.Game.Wecs.Systems.Actor;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Services
{
    internal class ActorTriggerMan : IActorTriggerMan
    {
        private readonly Dictionary<string, ActorTriggerCallback> callbackLookup = new Dictionary<string, ActorTriggerCallback>();

        public void RegisterCallback(string triggerType, ActorTriggerCallback callback)
        {
            if (!callbackLookup.TryAdd(triggerType, callback))
            {
                throw new InvalidOperationException($"Callback is already registered for trigger type '{triggerType}'.");
            }
        }

        public bool TryGetCallback(string triggerType, out ActorTriggerCallback callback)
        {
            return callbackLookup.TryGetValue(triggerType, out callback);
        }
    }
}
