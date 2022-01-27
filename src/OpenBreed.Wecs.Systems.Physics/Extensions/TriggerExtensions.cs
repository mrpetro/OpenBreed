using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityDirectionChanged(this ITriggerMan triggerMan, Entity entity, Action<Entity, DirectionChangedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<DirectionChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, DirectionChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<DirectionChangedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
    }
}
