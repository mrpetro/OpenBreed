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
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Sandbox.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityAction(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityActionEvent<PlayerActions>> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityActionEvent<PlayerActions>>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEntityLivesChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, LivesChangedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);

        public static void OnDestroyed(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, DestroyedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);

        public static void OnDamaged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, DamagedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);

        public static void OnInventoryChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, InventoryChangedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);
    }
}
