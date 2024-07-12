using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class EntityExtensions
    {
        public class EmitBuilder
        {
            private readonly EntityEmitterComponent emitterComponent;
            private readonly string templateName;
            private readonly Dictionary<string, object> options = new Dictionary<string, object>();

            internal EmitBuilder(EntityEmitterComponent emitterComponent, string templateName)
            {
                this.emitterComponent = emitterComponent;
                this.templateName = templateName;
            }

            public EmitBuilder SetOption(string name, object value)
            {
                options[name] = value;
                return this;
            }

            public void Finish()
            {
                emitterComponent.ToEmit.Add(new EntityEmit(templateName, options.ToDictionary(item => item.Key, item => item.Value)));
            }
        }

        public static EmitBuilder StartEmit(this IEntity entity, string templateName)
        {
            var emitterComponent = entity.Get<EntityEmitterComponent>();
            return new EmitBuilder(emitterComponent, templateName);
        }

        public static void Emit(this IEntity entity, string templateName)
        {
            var emitComponent = entity.Get<EntityEmitterComponent>();
            emitComponent.ToEmit.Add(new EntityEmit(templateName, new Dictionary<string, object>()));
        }

        public static int GetSourceEntityId(this IEntity entity)
        {
            return entity.Get<SourceEntityComponent>().EntityId;
        }

        public static int GetTimerId(this IEntity entity, string timerName)
        {
            var timerCmp = entity.Get<TimerComponent>();

            var timerId = timerCmp.Items.FindIndex(timer => timer.Name == timerName);

            if (timerId == -1)
                throw new InvalidOperationException($"Timer with name '{timerName}' not found.");

            return timerId;
        }

        public static int CreateTimer(this IEntity entity, string timerName)
        {
            var timerCmp = entity.Get<TimerComponent>();

            if (timerCmp.Items.Any(timer => timer.Name == timerName))
                throw new InvalidOperationException($"Timer with name '{timerName}' already exists.");

            var timerData = new TimerData(timerName, timerCmp.Items.Count, 0);
            timerData.Enabled = false;
            timerCmp.Items.Add(timerData);
            return timerData.TimerId;
        }

        public static void StartTimerEx(this IEntity entity, int timerId, double interval)
        {
            var timerCmp = entity.Get<TimerComponent>();
            var timerData = timerCmp.Items[timerId];
            timerData.Interval = interval;
            timerData.Enabled = true;
        }

        public static void StopTimer(this IEntity entity, int timerId)
        {
            var timerCmp = entity.Get<TimerComponent>();

            var timerData = timerCmp.Items.FirstOrDefault(item => item.TimerId == timerId);

            if (timerData is null)
                throw new InvalidOperationException($"Timer with ID '{timerId}' not found.");

            timerData.Enabled = false;
        }
    }
}
