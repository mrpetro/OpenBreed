using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Physics.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Physics.Systems.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityDirectionSet(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, DirectionSetEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);


        public static void OnEntityDirectionChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, DirectionChangedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);

        public static void OnEntityPositionChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, PositionChangedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);


        public static void OnEntityVelocityChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, VelocityChangedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);
    }
}
