using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class TriggerExtensions
    {
        public static void AnyKeyPressed(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, ControlFireChangedEventArgs> action, bool singleTime = false)
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
    }
}
