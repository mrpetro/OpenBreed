using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
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
        public static void OnEntityDirectionChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, DirectionChangedEventArgs> action,
            bool singleTime = false) => triggerMan.OnEvent(entity, action, singleTime);

        public static void OnEntityPositionChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, PositionChangedEventArgs> action,
            bool singleTime = false) => triggerMan.OnEvent(entity, action, singleTime);


        public static void OnEntityVelocityChanged(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, VelocityChangedEventArgs> action,
            bool singleTime = false) => triggerMan.OnEvent(entity, action, singleTime);
    }
}
