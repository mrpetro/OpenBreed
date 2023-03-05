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
        public static void Emit(this IEntity entity, string templateName)
        {
            var emitComponent = entity.Get<EntityEmitterComponent>();
            emitComponent.ToEmit.Add(new EntityEmit(templateName, new Dictionary<string, object>()));
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
