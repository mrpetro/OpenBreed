using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace OpenBreed.Wecs.Extensions
{
    public static class TriggerBuilderExtensions
    {
        #region Public Methods

        public static ITriggerBuilder OnEntity(this ITriggerBuilder triggerBuilder, IEntity entity)
        {
            //triggerBuilder.AddEqualsCondition(entity);

            return triggerBuilder;
        }

        #endregion Public Methods
    }

    public static class TriggerExtensions
    {
        #region Public Methods

        public static ITriggerBuilder OnEntity(this ITriggerMan triggerMan, IEntity entity)
        {
            //triggerMan.AddEqualsCondition(entity);

            return triggerMan.NewTrigger();
        }

        public static void OnEntityEnteredWorld(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityEnteredEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityEnteredEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEntityLeavingWorld(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityLeavingEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityLeavingEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEntityLeftWorld(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, EntityLeftEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityLeftEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEventEx<TEvent>(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, TEvent> action, bool singleTime = false) where TEvent : EntityEvent
        {
            triggerMan.CreateTrigger<TEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnEvent<TEvent>(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, TEvent> action, bool singleTime = false) where TEvent : EventArgs
        {
            triggerMan.EventsMan.Subscribe<TEvent>(ConditionalAction);

            void ConditionalAction(object sender, TEvent args)
            {
                if (!Equals(entity, sender))
                    return;

                if (singleTime)
                    triggerMan.EventsMan.Unsubscribe<TEvent>(ConditionalAction);

                action.Invoke(entity, args);
            }
        }
        public static void OnWorldInitialized(this ITriggerMan triggerMan, IWorld world, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldInitializedEventArgs>(
                (args) => Equals(world.Id, args.WorldId),
                (args) => action.Invoke(),
                singleTime);
        }
        public static ITriggerBuilder RunAction(this ITriggerBuilder triggerBuilder, Action action, bool singleTime = false)
        {
            //triggerMan.AddEqualsCondition(entity);

            return triggerBuilder;
        }

        #endregion Public Methods
    }
}
