using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityViewportResized(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, ViewportResizedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<ViewportResizedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, ViewportResizedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<ViewportResizedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
    }
}
