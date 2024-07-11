using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Gui.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Gui.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnCursorMoved(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, CursorMovedEntityEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<CursorMovedEntityEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }

        public static void OnCursorKeyPressed(this ITriggerMan triggerMan, IEntity entity, Action<IEntity, CursorKeyPressedEntityEvent> action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<CursorKeyPressedEntityEvent>(
                (args) => Equals(entity.Id, args.EntityId),
                (args) => action.Invoke(entity, args),
                singleTime);
        }
    }
}
