using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Animation.Systems.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Animation.Systems.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityAnimFinished(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, AnimFinishedEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<AnimFinishedEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
