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
        public static void StartTimer(this Entity entity, int timerId, double interval)
        {
            var timerCmp = entity.Get<TimerComponent>();

            var timerData = timerCmp.Items.FirstOrDefault(item => item.TimerId == timerId);

            if (timerData == null)
            {
                timerData = new TimerData(timerId, interval);
                timerCmp.Items.Add(timerData);
            }
            else
                timerData.Interval = interval;

            timerData.Enabled = true;
        }

        public static void StopTimer(this Entity entity, int timerId)
        {
            var timerCmp = entity.Get<TimerComponent>();

            var timerData = timerCmp.Items.FirstOrDefault(item => item.TimerId == timerId);

            if (timerData == null)
                return;

            timerData.Enabled = false;
        }
    }
}
