using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityControlFireChanged(this ITriggerMan triggerMan, Entity entity, Action<Entity, ControlFireChangedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<ControlFireChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, ControlFireChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<ControlFireChangedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnEntityControlDirectionChanged(this ITriggerMan triggerMan, Entity entity, Action<Entity, ControlDirectionChangedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<ControlDirectionChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, ControlDirectionChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<ControlDirectionChangedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }


        public static ITriggerBuilder OnControlDirectionChanged(this ITriggerBuilder triggerBuilder)
        {
            return triggerBuilder;
        }
    }
}
