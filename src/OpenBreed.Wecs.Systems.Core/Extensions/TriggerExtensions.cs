using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class TriggerExtensions
    {
        public static void AfterDelay(this ITriggerMan triggerMan, Entity entity, TimeSpan timeSpan, Action action, bool singleTime = false)
        {
            var rnd = new Random();
            var timerId = rnd.Next();
            triggerMan.CreateTrigger<TimerElapsedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId) && Equals(timerId, args.TimerId),
                (args) => action.Invoke(),
                singleTime);

            entity.StartTimer(timerId, timeSpan.TotalSeconds);
        }

        public static void OnEntityTimerElapsed(this ITriggerMan triggerMan, Entity entity, Action<Entity, TimerElapsedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<TimerElapsedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEntityTimerUpdate(this ITriggerMan triggerMan, Entity entity, Action<Entity, TimerUpdateEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<TimerUpdateEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnPausedWorld(this ITriggerMan triggerMan, Entity entity, Action<Entity, WorldPausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldPausedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnUnpausedWorld(this ITriggerMan triggerMan, Entity entity, Action<Entity, WorldUnpausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldUnpausedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
