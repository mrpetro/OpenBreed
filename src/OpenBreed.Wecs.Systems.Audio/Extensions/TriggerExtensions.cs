using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Audio.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class TriggerExtensions
    {
        private static void CallScript(object func)
        {
            var method = func.GetType().GetMethod("Call", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            method.Invoke(func, new object[] { new object[0] });
        }

        public static void OnSoundPlayed(this ITriggerMan triggerMan, Entity entity, int soundId, object func, bool singleTime = false)
        {
            triggerMan.CreateTrigger<SoundPlayEvent>(
                (args) => Equals(entity.Id, args.EntityId) && Equals(soundId, args.SoundId),
                (args) => CallScript(func),
                singleTime);
        }
    }
}
