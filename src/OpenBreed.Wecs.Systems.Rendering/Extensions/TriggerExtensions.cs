using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
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
