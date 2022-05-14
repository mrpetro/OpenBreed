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
        //public static void OnWorldInitialized(this ITriggerMan triggerMan, World world, Action<World, WorldInitializedEventArgs> action, bool singleTime = false)
        //{
        //    //var guid = Guid.NewGuid();

        //    triggerMan.EventsMan.Subscribe<WorldInitializedEventArgs>(ConditionalAction);

        //    void ConditionalAction(object sender, WorldInitializedEventArgs args)
        //    {
        //        if (!Equals(world.Id, args.WorldId))
        //            return;

        //        if (singleTime)
        //            triggerMan.EventsMan.Unsubscribe<WorldInitializedEventArgs>(ConditionalAction);

        //        action.Invoke(world, args);
        //    }
        //}

        public static void OnWorldInitialized(this ITriggerMan triggerMan, World world, Action action, bool singleTime = false)
        {
            //var guid = Guid.NewGuid();

            triggerMan.CreateTrigger<WorldInitializedEventArgs>((args) => Equals(world.Id, args.WorldId), action, singleTime);

            //void ConditionalAction(object sender, WorldInitializedEventArgs args)
            //{
            //    if (!Equals(world.Id, args.WorldId))
            //        return;

            //    if (singleTime)
            //        triggerMan.EventsMan.Unsubscribe<WorldInitializedEventArgs>(ConditionalAction);

            //    action.Invoke();
            //}
        }

        public static ITriggerBuilder OnEntity(this ITriggerMan triggerMan, Entity entity)
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
        public static ITriggerBuilder OnEntity(this ITriggerBuilder triggerBuilder, Entity entity)
        {
            //triggerBuilder.AddEqualsCondition(entity);

            return triggerBuilder;
        }
    }
}
