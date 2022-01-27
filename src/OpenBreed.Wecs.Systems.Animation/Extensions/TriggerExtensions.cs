using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityAnimFinished(this ITriggerMan triggerMan, Entity entity, Action<Entity, AnimFinishedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<AnimFinishedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, AnimFinishedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<AnimFinishedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
    }
}
