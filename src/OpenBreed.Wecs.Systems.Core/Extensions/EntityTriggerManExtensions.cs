using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class EntityTriggerManExtensions
    {
        #region Public Methods

        public static bool TryOnTrigger(this IEntityTriggerMan entityTriggerMan, string triggerName, IEntity actorEntity, IEntity triggerEntity)
        {
            var actionName = triggerEntity.GetOnTriggerAction(triggerName);

            if (actionName is not null && entityTriggerMan.TryGetCallback(triggerName, actionName, out EntityOnTriggerActionCallback actorTriggerCallback))
            {
                actorTriggerCallback.Invoke(actorEntity, triggerEntity);
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}