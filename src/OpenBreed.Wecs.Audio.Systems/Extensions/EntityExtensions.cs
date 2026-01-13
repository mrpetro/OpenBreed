using OpenBreed.Wecs.Audio.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Audio.Systems.Extensions
{
    public static class EntityExtensions
    {
        public static void EmitSound(this IEntity entity, int soundId)
        {
            var soundPlayer = entity.Get<SoundPlayerComponent>();

            if (soundPlayer.ToPlay.Contains(soundId))
                return;

            soundPlayer.ToPlay.Add(soundId);
        }
    }
}
