using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Audio.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Systems.Extensions
{
    public static class TriggerExtensions
    {
        public static void OnSoundPlayed(this ITriggerMan triggerMan, IEntity entity, int soundId, Action action, bool singleTime = false)
        {
            triggerMan.CreateTrigger<SoundPlayEvent>(
                (args) => Equals(entity.Id, args.EntityId) && Equals(soundId, args.SoundId),
                (args) => action.Invoke(),
                singleTime);
        }
    }
}
