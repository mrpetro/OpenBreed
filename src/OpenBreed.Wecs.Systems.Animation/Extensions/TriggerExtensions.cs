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
        public static void OnWorldEvent<TEventArgs>(this ITriggerMan triggerMan, World world, Action<World, TEventArgs> action, bool singleTime = false) where TEventArgs : EventArgs
        {
            triggerMan.EventsMan.Subscribe<TEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, TEventArgs args)
            {
                if (!Equals(world, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<TEventArgs>(ConditionalAction);

                action.Invoke(world, args);
            }
        }

        public static void OnEntityEvent<TEventArgs>(this ITriggerMan triggerMan, Entity entity, Action<Entity, TEventArgs> action, bool singleTime = false) where TEventArgs : EventArgs
        {
            triggerMan.EventsMan.Subscribe<TEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, TEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<TEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnEntityAnimFinished(this ITriggerMan triggerMan, Entity entity, Action<Entity, AnimFinishedEventArgs> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<AnimFinishedEventArgs>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
