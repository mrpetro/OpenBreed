using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenBreed.Wecs.Control.Components;
using OpenBreed.Wecs.Control.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Control.Systems.Extensions
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

        public static void OnEntityAnimFinished(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, AnimFinishedEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<AnimFinishedEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
