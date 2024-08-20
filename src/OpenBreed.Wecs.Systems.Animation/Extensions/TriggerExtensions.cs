using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
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
