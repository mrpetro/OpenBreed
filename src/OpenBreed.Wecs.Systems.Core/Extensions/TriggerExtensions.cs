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
        public static void AfterDelay(this ITriggerMan triggerMan, TimeSpan timeSpan, object func)
        {
            Task.Delay((int)timeSpan.TotalMilliseconds).Wait();

            var method = func.GetType().GetMethod("Call", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            method.Invoke(func, new object[] { new object[0] });
        }

        public static void OnEntityTimerElapsed(this ITriggerMan triggerMan, Entity entity, Action<Entity, TimerElapsedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<TimerElapsedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, TimerElapsedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<TimerElapsedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnEntityTimerUpdate(this ITriggerMan triggerMan, Entity entity, Action<Entity, TimerUpdateEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<TimerUpdateEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, TimerUpdateEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<TimerUpdateEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnPausedWorld(this ITriggerMan triggerMan, Entity entity, Action<Entity, WorldPausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<WorldPausedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, WorldPausedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<WorldPausedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnUnpausedWorld(this ITriggerMan triggerMan, Entity entity, Action<Entity, WorldUnpausedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<WorldUnpausedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, WorldUnpausedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<WorldUnpausedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
    }
}
