using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Rendering.Systems.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnEntityViewportResized(
            this ITriggerMan triggerMan,
            IEntity entity,
            Action<IEntity, ViewportResizedEvent> action,
            bool singleTime = false) => triggerMan.OnEntityEvent(entity, action, singleTime);
    }
}
