using OpenBreed.Wecs.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
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

        #endregion Public Methods
    }
}