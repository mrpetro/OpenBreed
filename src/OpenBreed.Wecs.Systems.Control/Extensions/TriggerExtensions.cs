using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Input.Interface.Events;
using OpenBreed.Wecs.Components.Control;
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
        public static void AnyKeyPressed(this ITriggerMan triggerMan, Action<KeyDownEvent> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<KeyDownEvent>(ConditionalAction);

            void ConditionalAction(KeyDownEvent args)
            {
                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<KeyDownEvent>(ConditionalAction);

                action.Invoke(args);
            }
        }

        public static void OnEntityFollow(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityFollowEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityFollowEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
