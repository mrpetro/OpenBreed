using OpenBreed.Core;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnWorldPaused(this ITriggerMan triggerMan, int worldId, Action<int> action, bool singleTime = false)
        {
            //triggerMan.EventsMan.
        }
    }
}
