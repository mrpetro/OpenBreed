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

namespace OpenBreed.Wecs.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityEnteredWorld2(this ITriggerMan triggerMan, IEntity entity, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityEnteredEventArgs>((args) => Equals(entity.Id, args.EntityId), (args) => action.Invoke(), singleTime);
        }

        public static void OnEntityEnteredWorld(this ITriggerMan triggerMan, IEntity entity, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityEnteredEventArgs>((args) => Equals(entity.Id, args.EntityId), (args) => action.Invoke(), singleTime);
        }

        public static void OnEntityLeftWorld(this ITriggerMan triggerMan, IEntity entity, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<EntityLeftEventArgs>((args) => Equals(entity.Id, args.EntityId), (args) => action.Invoke(), singleTime);
        }

        public static void OnWorldInitialized(this ITriggerMan triggerMan, World world, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<WorldInitializedEventArgs>((args) => Equals(world.Id, args.WorldId), (args) => action.Invoke(), singleTime);
        }

        public static ITriggerBuilder OnEntity(this ITriggerMan triggerMan, IEntity entity)
        {
            //triggerMan.AddEqualsCondition(entity);

            return triggerMan.NewTrigger();
        }

        public static ITriggerBuilder RunAction(this ITriggerBuilder triggerBuilder, Action action, bool singleTime = false)
        {
            //triggerMan.AddEqualsCondition(entity);

            return triggerBuilder;
        }
    }

    public static class TriggerBuilderExtensions
    {
        public static ITriggerBuilder OnEntity(this ITriggerBuilder triggerBuilder, IEntity entity)
        {
            //triggerBuilder.AddEqualsCondition(entity);

            return triggerBuilder;
        }
    }
}
