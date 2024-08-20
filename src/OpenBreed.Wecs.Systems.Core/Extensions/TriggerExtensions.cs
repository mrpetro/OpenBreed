using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class TriggerExtensions
    {
        public static void EveryFrame(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityFrameEvent> action, int framesNo, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<EntityFrameEvent>(ConditionalAction);

            void ConditionalAction(EntityFrameEvent args)
            {
                if (!Equals(entity.Id, args.EntityId))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<EntityFrameEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }

            entity.Get<FrameComponent>().Target = framesNo;
        }

        public static void AfterDelay(this ITriggerMan triggerMan, IEntity entity, int timerId, TimeSpan timeSpan, Action<IEntity, TimerElapsedEventArgs>  action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<TimerElapsedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId) && Equals(timerId, args.TimerId),
                (args) => action.Invoke(entity, args),
                singleTime);

            entity.StartTimerEx(timerId, timeSpan.TotalSeconds);
        }

        public static void OnEntityTimerElapsed(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, TimerElapsedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<TimerElapsedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEntityTimerUpdate(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, TimerUpdateEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<TimerUpdateEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnPausedWorld(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, WorldPausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldPausedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnUnpausedWorld(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, WorldUnpausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldUnpausedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEmitEntity(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EmitEntityEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EmitEntityEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnLifetimeEnd(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, LifetimeEndEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<LifetimeEndEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
