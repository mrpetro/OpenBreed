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
        public static void OnSoundPlayed(this ITriggerMan triggerMan, Entity entity, int soundId, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<SoundPlayEvent>(
                (args) => Equals(entity.Id, args.EntityId) && Equals(soundId, args.SoundId),
                (args) => action.Invoke(),
                singleTime);
        }
    }
}
