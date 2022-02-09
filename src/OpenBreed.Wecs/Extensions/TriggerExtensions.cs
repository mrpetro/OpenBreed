using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class TriggerExtensions
    {
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
