using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Systems.Extensions
{
    public static class EntityTriggerManExtensions
    {
        #region Public Methods

        public static bool TryOnTrigger<TSystem>(this IEntityTriggerMan entityTriggerMan,
            string triggerName,
            IEntity actorEntity,
            IEntity triggerEntity, Action<TSystem> callback) where TSystem : class, IActionOnTriggerSystem
        {
            var actionNames = triggerEntity.GetActionsOnTrigger(triggerName);

            bool result = false;

            foreach (var actionName in actionNames)
            {
                if (entityTriggerMan.TryGetTriggerSystem(triggerName, actionName, out TSystem system))
                {
                    callback.Invoke(system);
                    result = true;
                }
            }

            return result;
        }

        public static bool TryOnTrigger(this IEntityTriggerMan entityTriggerMan, string triggerName, IEntity actorEntity, IEntity triggerEntity)
        {
            var actionNames = triggerEntity.GetActionsOnTrigger(triggerName);

            bool result = true;

            foreach (var actionName in actionNames)
            {
                if (entityTriggerMan.TryGetCallback(triggerName, actionName, out EntityOnTriggerActionCallback actorTriggerCallback))
                {
                    actorTriggerCallback.Invoke(actorEntity, triggerEntity);
                    result = false;
                }
            }

            return result;
        }

        #endregion Public Methods
    }
}