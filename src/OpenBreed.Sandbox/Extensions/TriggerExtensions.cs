using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Sandbox.Wecs.Events;
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
        public static void OnEntityLivesChanged(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, LivesChangedEvent> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<LivesChangedEvent>(ConditionalAction);

            void ConditionalAction(object sender, LivesChangedEvent args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<LivesChangedEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnEntityAction(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityActionEvent<PlayerActions>> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityActionEvent<PlayerActions>>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }


        public static void OnDamagedEntity(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, DamagedEvent> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<DamagedEvent>(ConditionalAction);

            void ConditionalAction(object sender, DamagedEvent args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<DamagedEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnInventoryChanged(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, InventoryChangedEvent> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<InventoryChangedEvent>(ConditionalAction);

            void ConditionalAction(object sender, InventoryChangedEvent args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<InventoryChangedEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

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
