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
        public static void OnEntityDirectionChanged(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, DirectionChangedEventArgs> action, bool singleTime = false)
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

        public static void OnEntityDirectionChangedEx(this ITriggerMan triggerMan, IEntity entity, Func<IEntity, DirectionChangedEventArgs, bool> action)
        {
            triggerMan.EventsMan.Subscribe<DirectionChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, DirectionChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                var result = action.Invoke(entity, args);

                if (result)
                    triggerMan.EventsMan.Unsubscribe<DirectionChangedEventArgs>(ConditionalAction);
            }
        }

        public static void OnEntityPositionChanged(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, PositionChangedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<PositionChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, PositionChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<PositionChangedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }

        public static void OnEntityPositionChanged(this ITriggerMan triggerMan, IEntity entity, Func<IEntity, PositionChangedEventArgs, bool> action)
        {
            triggerMan.EventsMan.Subscribe<PositionChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, PositionChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                var result = action.Invoke(entity, args);

                if (result)
                    triggerMan.EventsMan.Unsubscribe<PositionChangedEventArgs>(ConditionalAction);
            }
        }

        //public static void OnEntityVelocityChanged(this ITriggerMan triggerMan, IEntity entity, Func<IEntity, VelocityChangedEventArgs, bool> action)
        //{
        //    triggerMan.EventsMan.Subscribe<VelocityChangedEventArgs>(ConditionalAction);

        //    void ConditionalAction(object sender, VelocityChangedEventArgs args)
        //    {
        //        if (!Equals(entity, sender))
        //            return;

        //        var result = action.Invoke(entity, args);

        //        if (result)
        //            triggerMan.EventsMan.Unsubscribe<VelocityChangedEventArgs>(ConditionalAction);
        //    }
        //}

        public static void OnEntityVelocityChanged(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, VelocityChangedEventArgs> action, bool singleTime = false)
        {
            triggerMan.EventsMan.Subscribe<VelocityChangedEventArgs>(ConditionalAction);

            void ConditionalAction(object sender, VelocityChangedEventArgs args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<VelocityChangedEventArgs>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
    }
}
