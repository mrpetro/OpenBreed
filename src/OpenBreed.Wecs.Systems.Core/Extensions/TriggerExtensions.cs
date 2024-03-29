﻿using OpenBreed.Core.Managers;
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
        public static void EveryFrame(this ITriggerMan triggerMan, Entity entity, Action<Entity, EntityFrameEvent> action, int framesNo, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<EntityFrameEvent>(ConditionalAction);

            void ConditionalAction(object sender, EntityFrameEvent args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<EntityFrameEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }

            entity.Get<FrameComponent>().Target = framesNo;
        }

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
